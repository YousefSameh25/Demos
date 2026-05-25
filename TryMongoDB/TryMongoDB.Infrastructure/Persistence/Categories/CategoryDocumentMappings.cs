using TryMongoDB.Domain.Categories;

namespace TryMongoDB.Infrastructure.Persistence.Categories;

internal static class CategoryDocumentMappings
{
    public static Category ToDomain(this CategoryDocument document) =>
        Category.Rehydrate(
            document.Id,
            document.Name,
            document.Slug,
            document.Description,
            document.IsActive,
            document.CreatedAtUtc,
            document.UpdatedAtUtc);

    public static CategoryDocument ToDocument(this Category category) =>
        new()
        {
            Id = category.Id,
            Name = category.Name,
            Slug = category.Slug,
            Description = category.Description,
            IsActive = category.IsActive,
            CreatedAtUtc = category.CreatedAtUtc,
            UpdatedAtUtc = category.UpdatedAtUtc
        };
}
