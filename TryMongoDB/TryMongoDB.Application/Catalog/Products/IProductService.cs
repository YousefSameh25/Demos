using TryMongoDB.Application.Common;

namespace TryMongoDB.Application.Catalog.Products;

public interface IProductService
{
    Task<ApplicationResult<ProductDto>> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken);
    Task<ApplicationResult<ProductVariantDto>> AddVariantAsync(string productId, CreateProductVariantRequest request, CancellationToken cancellationToken);
    Task<ApplicationResult<ProductDto>> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<ProductDto>> ListAsync(ProductFilter filter, CancellationToken cancellationToken);
    Task<ApplicationResult<ProductDto>> UpdateAsync(string id, UpdateProductRequest request, CancellationToken cancellationToken);
    Task<ApplicationResult<bool>> DeleteAsync(string id, CancellationToken cancellationToken);
}
