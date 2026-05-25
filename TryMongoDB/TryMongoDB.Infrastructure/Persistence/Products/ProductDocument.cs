namespace TryMongoDB.Infrastructure.Persistence.Products;

public sealed class ProductDocument
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CategoryId { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }

    // Product-level custom fields live here, for example: brand, warranty, care instructions.
    public Dictionary<string, string> Attributes { get; set; } = [];

    public bool IsActive { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}
