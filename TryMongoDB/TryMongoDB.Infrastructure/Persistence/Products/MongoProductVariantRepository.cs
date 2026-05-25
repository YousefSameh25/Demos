using MongoDB.Driver;
using TryMongoDB.Application.Catalog.Products;
using TryMongoDB.Domain.Products;

namespace TryMongoDB.Infrastructure.Persistence.Products;

public sealed class MongoProductVariantRepository(MongoCatalogContext context) : IProductVariantRepository
{
    public async Task<IReadOnlyCollection<ProductVariant>> ListByProductIdAsync(
        string productId,
        CancellationToken cancellationToken)
    {
        var documents = await context.ProductVariants
            .Find(variant => variant.ProductId == productId)
            .SortBy(variant => variant.Sku)
            .ToListAsync(cancellationToken);

        return documents.Select(variant => variant.ToDomain()).ToArray();
    }

    public async Task<IReadOnlyCollection<ProductVariant>> ListByProductIdsAsync(
        IReadOnlyCollection<string> productIds,
        CancellationToken cancellationToken)
    {
        if (productIds.Count == 0)
        {
            return [];
        }

        var documents = await context.ProductVariants
            .Find(variant => productIds.Contains(variant.ProductId))
            .SortBy(variant => variant.Sku)
            .ToListAsync(cancellationToken);

        return documents.Select(variant => variant.ToDomain()).ToArray();
    }

    public async Task<IReadOnlyCollection<string>> ListProductIdsByPriceRangeAsync(
        decimal? minPrice,
        decimal? maxPrice,
        CancellationToken cancellationToken)
    {
        var builder = Builders<ProductVariantDocument>.Filter;
        var filters = new List<FilterDefinition<ProductVariantDocument>>();

        if (minPrice is not null)
        {
            filters.Add(builder.Gte(variant => variant.Price, minPrice.Value));
        }

        if (maxPrice is not null)
        {
            filters.Add(builder.Lte(variant => variant.Price, maxPrice.Value));
        }

        var filter = filters.Count == 0 ? builder.Empty : builder.And(filters);
        var productIds = await context.ProductVariants
            .Distinct(variant => variant.ProductId, filter)
            .ToListAsync(cancellationToken);

        return productIds;
    }

    public Task<bool> ExistsBySkuAsync(string sku, CancellationToken cancellationToken) =>
        context.ProductVariants
            .Find(variant => variant.Sku == sku.Trim())
            .AnyAsync(cancellationToken);

    public Task<bool> ExistsBySkuOnAnotherProductAsync(
        string sku,
        string productId,
        CancellationToken cancellationToken) =>
        context.ProductVariants
            .Find(variant => variant.Sku == sku.Trim() && variant.ProductId != productId)
            .AnyAsync(cancellationToken);

    public Task AddManyAsync(IReadOnlyCollection<ProductVariant> variants, CancellationToken cancellationToken)
    {
        if (variants.Count == 0)
        {
            return Task.CompletedTask;
        }

        return context.ProductVariants.InsertManyAsync(
            variants.Select(variant => variant.ToDocument()),
            cancellationToken: cancellationToken);
    }

    public async Task ReplaceForProductAsync(
        string productId,
        IReadOnlyCollection<ProductVariant> variants,
        CancellationToken cancellationToken)
    {
        await DeleteByProductIdAsync(productId, cancellationToken);
        await AddManyAsync(variants, cancellationToken);
    }

    public Task DeleteByProductIdAsync(string productId, CancellationToken cancellationToken) =>
        context.ProductVariants.DeleteManyAsync(variant => variant.ProductId == productId, cancellationToken);
}
