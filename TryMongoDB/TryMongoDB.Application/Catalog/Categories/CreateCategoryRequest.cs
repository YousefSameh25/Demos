namespace TryMongoDB.Application.Catalog.Categories;

public sealed record CreateCategoryRequest(string Name, string Slug, string Description);
