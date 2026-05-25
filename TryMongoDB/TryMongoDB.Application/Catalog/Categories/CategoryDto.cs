namespace TryMongoDB.Application.Catalog.Categories;

public sealed record CategoryDto(
    string Id,
    string Name,
    string Slug,
    string Description,
    bool IsActive,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);
