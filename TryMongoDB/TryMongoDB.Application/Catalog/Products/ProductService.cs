using TryMongoDB.Application.Catalog.Categories;
using TryMongoDB.Application.Common;
using TryMongoDB.Domain.Products;

namespace TryMongoDB.Application.Catalog.Products;

public sealed class ProductService(
    IProductRepository products,
    IProductVariantRepository variants,
    ICategoryRepository categories) : IProductService
{
    public async Task<ApplicationResult<ProductDto>> CreateAsync(
        CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        if (await categories.GetByIdAsync(request.CategoryId, cancellationToken) is null)
        {
            return ApplicationResult<ProductDto>.Failure(ApplicationErrorType.Validation, "Product category does not exist.");
        }

        if (request.Variants is null || request.Variants.Count == 0)
        {
            return ApplicationResult<ProductDto>.Failure(ApplicationErrorType.Validation, "Product must have at least one variant.");
        }

        var duplicateSku = FindDuplicateSku(request.Variants);
        if (duplicateSku is not null)
        {
            return ApplicationResult<ProductDto>.Failure(ApplicationErrorType.Validation, $"Product variant SKU '{duplicateSku}' is duplicated.");
        }

        foreach (var sku in request.Variants.Select(variant => variant.Sku))
        {
            if (await variants.ExistsBySkuAsync(sku, cancellationToken))
            {
                return ApplicationResult<ProductDto>.Failure(ApplicationErrorType.Conflict, $"Product variant SKU '{sku}' already exists.");
            }
        }

        var product = Product.Create(
            request.Name,
            request.Description,
            request.CategoryId,
            request.ImageUrl,
            request.Attributes,
            DateTime.UtcNow);

        await products.AddAsync(product, cancellationToken);

        var productVariants = request.Variants.Select(variant => ToProductVariant(product.Id, variant)).ToArray();
        await variants.AddManyAsync(productVariants, cancellationToken);

        return ApplicationResult<ProductDto>.Success(product.ToDto(productVariants));
    }

    public async Task<ApplicationResult<ProductDto>> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var product = await products.GetByIdAsync(id, cancellationToken);

        if (product is null)
        {
            return ApplicationResult<ProductDto>.Failure(ApplicationErrorType.NotFound, "Product was not found.");
        }

        var productVariants = await variants.ListByProductIdAsync(product.Id, cancellationToken);

        return ApplicationResult<ProductDto>.Success(product.ToDto(productVariants));
    }

    public async Task<IReadOnlyCollection<ProductDto>> ListAsync(ProductFilter filter, CancellationToken cancellationToken)
    {
        if (filter.MinPrice is not null || filter.MaxPrice is not null)
        {
            var matchingProductIds = await variants.ListProductIdsByPriceRangeAsync(
                filter.MinPrice,
                filter.MaxPrice,
                cancellationToken);

            if (matchingProductIds.Count == 0)
            {
                return [];
            }

            filter = filter with { ProductIds = matchingProductIds };
        }

        var result = await products.ListAsync(filter, cancellationToken);
        var resultVariants = await variants.ListByProductIdsAsync(
            result.Select(product => product.Id).ToArray(),
            cancellationToken);
        var variantsByProductId = resultVariants
            .GroupBy(variant => variant.ProductId)
            .ToDictionary(group => group.Key, group => (IReadOnlyCollection<ProductVariant>)group.ToArray());

        return result
            .Select(product => product.ToDto(
                variantsByProductId.TryGetValue(product.Id, out var productVariants)
                    ? productVariants
                    : []))
            .ToArray();
    }

    public async Task<ApplicationResult<ProductDto>> UpdateAsync(
        string id,
        UpdateProductRequest request,
        CancellationToken cancellationToken)
    {
        var product = await products.GetByIdAsync(id, cancellationToken);
        if (product is null)
        {
            return ApplicationResult<ProductDto>.Failure(ApplicationErrorType.NotFound, "Product was not found.");
        }

        if (await categories.GetByIdAsync(request.CategoryId, cancellationToken) is null)
        {
            return ApplicationResult<ProductDto>.Failure(ApplicationErrorType.Validation, "Product category does not exist.");
        }

        if (request.Variants is null || request.Variants.Count == 0)
        {
            return ApplicationResult<ProductDto>.Failure(ApplicationErrorType.Validation, "Product must have at least one variant.");
        }

        var duplicateSku = FindDuplicateSku(request.Variants);
        if (duplicateSku is not null)
        {
            return ApplicationResult<ProductDto>.Failure(ApplicationErrorType.Validation, $"Product variant SKU '{duplicateSku}' is duplicated.");
        }

        foreach (var sku in request.Variants.Select(variant => variant.Sku))
        {
            if (await variants.ExistsBySkuOnAnotherProductAsync(sku, product.Id, cancellationToken))
            {
                return ApplicationResult<ProductDto>.Failure(ApplicationErrorType.Conflict, $"Product variant SKU '{sku}' already exists.");
            }
        }

        product.UpdateDetails(
            request.Name,
            request.Description,
            request.CategoryId,
            request.ImageUrl,
            request.Attributes,
            request.IsActive,
            DateTime.UtcNow);

        await products.UpdateAsync(product, cancellationToken);
        var productVariants = request.Variants.Select(variant => ToProductVariant(product.Id, variant)).ToArray();
        await variants.ReplaceForProductAsync(product.Id, productVariants, cancellationToken);

        return ApplicationResult<ProductDto>.Success(product.ToDto(productVariants));
    }

    public async Task<ApplicationResult<bool>> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        var product = await products.GetByIdAsync(id, cancellationToken);
        if (product is null)
        {
            return ApplicationResult<bool>.Failure(ApplicationErrorType.NotFound, "Product was not found.");
        }

        await variants.DeleteByProductIdAsync(id, cancellationToken);
        await products.DeleteAsync(id, cancellationToken);

        return ApplicationResult<bool>.Success(true);
    }

    private static ProductVariant ToProductVariant(string productId, ProductVariantRequest request) =>
        string.IsNullOrWhiteSpace(request.Id)
            ? ProductVariant.Create(
                productId,
                request.Sku,
                request.Price,
                request.Currency,
                request.StockQuantity,
                request.Attributes,
                request.IsActive)
            : ProductVariant.Rehydrate(
                request.Id,
                productId,
                request.Sku,
                request.Price,
                request.Currency,
                request.StockQuantity,
                request.Attributes,
                request.IsActive);

    private static string? FindDuplicateSku(IReadOnlyCollection<ProductVariantRequest> variants) =>
        variants
            .Where(variant => !string.IsNullOrWhiteSpace(variant.Sku))
            .GroupBy(variant => variant.Sku.Trim(), StringComparer.OrdinalIgnoreCase)
            .FirstOrDefault(group => group.Count() > 1)
            ?.Key;
}
