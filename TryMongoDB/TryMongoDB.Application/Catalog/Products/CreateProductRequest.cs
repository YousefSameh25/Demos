namespace TryMongoDB.Application.Catalog.Products;

public sealed record CreateProductRequest(
    string Name,
    string Description,
    string CategoryId,
    string? ImageUrl,
    IReadOnlyDictionary<string, string>? Attributes,
    IReadOnlyCollection<ProductVariantRequest> Variants);

public sealed record ProductVariantRequest(
    string Sku,
    decimal Price,
    string Currency,
    int StockQuantity,
    IReadOnlyDictionary<string, string>? Attributes,
    bool IsActive = true,
    string? Id = null);
