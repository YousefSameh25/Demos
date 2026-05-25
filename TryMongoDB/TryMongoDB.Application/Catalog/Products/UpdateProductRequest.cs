namespace TryMongoDB.Application.Catalog.Products;

public sealed record UpdateProductRequest(
    string Name,
    string Description,
    string CategoryId,
    string? ImageUrl,
    IReadOnlyDictionary<string, string>? Attributes,
    IReadOnlyCollection<ProductVariantRequest> Variants,
    bool IsActive);
