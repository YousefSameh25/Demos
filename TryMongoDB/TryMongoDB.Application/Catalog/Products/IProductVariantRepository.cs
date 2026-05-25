using TryMongoDB.Domain.Products;

namespace TryMongoDB.Application.Catalog.Products;

public interface IProductVariantRepository
{
    Task<IReadOnlyCollection<ProductVariant>> ListByProductIdAsync(string productId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<ProductVariant>> ListByProductIdsAsync(IReadOnlyCollection<string> productIds, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<string>> ListProductIdsByPriceRangeAsync(decimal? minPrice, decimal? maxPrice, CancellationToken cancellationToken);
    Task<bool> ExistsBySkuAsync(string sku, CancellationToken cancellationToken);
    Task<bool> ExistsBySkuOnAnotherProductAsync(string sku, string productId, CancellationToken cancellationToken);
    Task AddAsync(ProductVariant variant, CancellationToken cancellationToken);
    Task AddManyAsync(IReadOnlyCollection<ProductVariant> variants, CancellationToken cancellationToken);
    Task ReplaceForProductAsync(string productId, IReadOnlyCollection<ProductVariant> variants, CancellationToken cancellationToken);
    Task DeleteByProductIdAsync(string productId, CancellationToken cancellationToken);
}
