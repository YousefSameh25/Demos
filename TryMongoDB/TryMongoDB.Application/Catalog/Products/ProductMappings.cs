using TryMongoDB.Domain.Products;

namespace TryMongoDB.Application.Catalog.Products;

internal static class ProductMappings
{
    public static ProductDto ToDto(this Product product, IReadOnlyCollection<ProductVariant> variants) =>
        new(
            product.Id,
            product.Name,
            product.Description,
            product.CategoryId,
            product.ImageUrl,
            product.Attributes,
            variants.Select(variant => variant.ToDto()).ToArray(),
            product.IsActive,
            product.CreatedAtUtc,
            product.UpdatedAtUtc);

    public static ProductVariantDto ToDto(this ProductVariant variant) =>
        new(
            variant.Id,
            variant.Sku,
            variant.Price,
            variant.Currency,
            variant.StockQuantity,
            variant.Attributes,
            variant.IsActive);
}
