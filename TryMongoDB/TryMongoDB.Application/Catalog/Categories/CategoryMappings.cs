using TryMongoDB.Domain.Categories;

namespace TryMongoDB.Application.Catalog.Categories;

internal static class CategoryMappings
{
    public static CategoryDto ToDto(this Category category) =>
        new(
            category.Id,
            category.Name,
            category.Slug,
            category.Description,
            category.IsActive,
            category.CreatedAtUtc,
            category.UpdatedAtUtc);
}
