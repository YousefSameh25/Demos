namespace TryMongoDB.Application.Catalog.Products;

public sealed record ProductDto(
    string Id,
    string Name,
    string Description,
    string CategoryId,
    string? ImageUrl,
    IReadOnlyDictionary<string, string> Attributes,
    IReadOnlyCollection<ProductVariantDto> Variants,
    bool IsActive,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);

public sealed record ProductVariantDto(
    string Id,
    string Sku,
    decimal Price,
    string Currency,
    int StockQuantity,
    IReadOnlyDictionary<string, string> Attributes,
    bool IsActive);
