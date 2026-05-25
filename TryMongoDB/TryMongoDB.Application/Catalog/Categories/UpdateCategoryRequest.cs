namespace TryMongoDB.Application.Catalog.Categories;

public sealed record UpdateCategoryRequest(string Name, string Description, bool IsActive);
