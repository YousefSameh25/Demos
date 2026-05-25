using TryMongoDB.Domain.Products;

namespace TryMongoDB.Application.Catalog.Products;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Product>> ListAsync(ProductFilter filter, CancellationToken cancellationToken);
    Task AddAsync(Product product, CancellationToken cancellationToken);
    Task UpdateAsync(Product product, CancellationToken cancellationToken);
    Task DeleteAsync(string id, CancellationToken cancellationToken);
}
