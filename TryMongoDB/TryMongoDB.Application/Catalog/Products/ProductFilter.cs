namespace TryMongoDB.Application.Catalog.Products;

public sealed record ProductFilter(
    string? CategoryId,
    bool? IsActive,
    decimal? MinPrice,
    decimal? MaxPrice,
    string? SearchText,
    IReadOnlyCollection<string>? ProductIds = null);
