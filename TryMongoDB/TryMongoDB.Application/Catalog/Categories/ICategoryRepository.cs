using TryMongoDB.Domain.Categories;

namespace TryMongoDB.Application.Catalog.Categories;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Category>> ListAsync(CancellationToken cancellationToken);
    Task<bool> ExistsBySlugAsync(string slug, CancellationToken cancellationToken);
    Task AddAsync(Category category, CancellationToken cancellationToken);
    Task UpdateAsync(Category category, CancellationToken cancellationToken);
    Task DeleteAsync(string id, CancellationToken cancellationToken);
}
