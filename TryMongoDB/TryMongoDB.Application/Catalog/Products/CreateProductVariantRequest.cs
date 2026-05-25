namespace TryMongoDB.Application.Catalog.Products;

public sealed record CreateProductVariantRequest(
    string Sku,
    decimal Price,
    string Currency,
    int StockQuantity,
    IReadOnlyDictionary<string, string>? Attributes,
    bool IsActive = true);
