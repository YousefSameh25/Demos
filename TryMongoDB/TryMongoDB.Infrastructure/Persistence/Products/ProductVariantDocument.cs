namespace TryMongoDB.Infrastructure.Persistence.Products;

public sealed class ProductVariantDocument
{
    public string Id { get; set; } = string.Empty;
    public string ProductId { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Currency { get; set; } = string.Empty;
    public int StockQuantity { get; set; }
    public Dictionary<string, string> Attributes { get; set; } = [];
    public bool IsActive { get; set; }
}
