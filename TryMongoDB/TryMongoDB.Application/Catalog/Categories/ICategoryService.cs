using TryMongoDB.Application.Common;

namespace TryMongoDB.Application.Catalog.Categories;

public interface ICategoryService
{
    Task<ApplicationResult<CategoryDto>> CreateAsync(CreateCategoryRequest request, CancellationToken cancellationToken);
    Task<ApplicationResult<CategoryDto>> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<CategoryDto>> ListAsync(CancellationToken cancellationToken);
    Task<ApplicationResult<CategoryDto>> UpdateAsync(string id, UpdateCategoryRequest request, CancellationToken cancellationToken);
    Task<ApplicationResult<bool>> DeleteAsync(string id, CancellationToken cancellationToken);
}
