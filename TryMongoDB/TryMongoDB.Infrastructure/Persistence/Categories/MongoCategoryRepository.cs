using MongoDB.Driver;
using TryMongoDB.Application.Catalog.Categories;
using TryMongoDB.Domain.Categories;

namespace TryMongoDB.Infrastructure.Persistence.Categories;

public sealed class MongoCategoryRepository(MongoCatalogContext context) : ICategoryRepository
{
    public async Task<Category?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var document = await context.Categories
            .Find(category => category.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        return document?.ToDomain();
    }

    public async Task<IReadOnlyCollection<Category>> ListAsync(CancellationToken cancellationToken)
    {
        var documents = await context.Categories
            .Find(Builders<CategoryDocument>.Filter.Empty)
            .SortBy(category => category.Name)
            .ToListAsync(cancellationToken);

        return documents.Select(category => category.ToDomain()).ToArray();
    }

    public Task<bool> ExistsBySlugAsync(string slug, CancellationToken cancellationToken) =>
        context.Categories
            .Find(category => category.Slug == slug.Trim().ToLowerInvariant())
            .AnyAsync(cancellationToken);

    public Task AddAsync(Category category, CancellationToken cancellationToken) =>
        context.Categories.InsertOneAsync(category.ToDocument(), cancellationToken: cancellationToken);

    public Task UpdateAsync(Category category, CancellationToken cancellationToken) =>
        context.Categories.ReplaceOneAsync(
            existing => existing.Id == category.Id,
            category.ToDocument(),
            cancellationToken: cancellationToken);

    public Task DeleteAsync(string id, CancellationToken cancellationToken) =>
        context.Categories.DeleteOneAsync(category => category.Id == id, cancellationToken);
}
