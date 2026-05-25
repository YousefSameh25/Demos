namespace TryMongoDB.Domain.Products;

public sealed class Product
{
    private Product(
        string id,
        string name,
        string description,
        string categoryId,
        string? imageUrl,
        IReadOnlyDictionary<string, string> attributes,
        bool isActive,
        DateTime createdAtUtc,
        DateTime updatedAtUtc)
    {
        Id = id;
        Name = name;
        Description = description;
        CategoryId = categoryId;
        ImageUrl = imageUrl;
        Attributes = attributes;
        IsActive = isActive;
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = updatedAtUtc;
    }

    public string Id { get; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string CategoryId { get; private set; }
    public string? ImageUrl { get; private set; }
    public IReadOnlyDictionary<string, string> Attributes { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAtUtc { get; }
    public DateTime UpdatedAtUtc { get; private set; }

    public static Product Create(
        string name,
        string description,
        string categoryId,
        string? imageUrl,
        IReadOnlyDictionary<string, string>? attributes,
        DateTime utcNow)
    {
        Validate(name, description, categoryId);

        return new Product(
            Guid.NewGuid().ToString("N"),
            name.Trim(),
            description.Trim(),
            categoryId.Trim(),
            string.IsNullOrWhiteSpace(imageUrl) ? null : imageUrl.Trim(),
            NormalizeAttributes(attributes, "Product"),
            true,
            utcNow,
            utcNow);
    }

    public static Product Rehydrate(
        string id,
        string name,
        string description,
        string categoryId,
        string? imageUrl,
        IReadOnlyDictionary<string, string>? attributes,
        bool isActive,
        DateTime createdAtUtc,
        DateTime updatedAtUtc)
    {
        Validate(name, description, categoryId);

        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("Product id is required.", nameof(id));
        }

        return new Product(
            id.Trim(),
            name.Trim(),
            description.Trim(),
            categoryId.Trim(),
            string.IsNullOrWhiteSpace(imageUrl) ? null : imageUrl.Trim(),
            NormalizeAttributes(attributes, "Product"),
            isActive,
            createdAtUtc,
            updatedAtUtc);
    }

    public void UpdateDetails(
        string name,
        string description,
        string categoryId,
        string? imageUrl,
        IReadOnlyDictionary<string, string>? attributes,
        bool isActive,
        DateTime utcNow)
    {
        Validate(name, description, categoryId);

        Name = name.Trim();
        Description = description.Trim();
        CategoryId = categoryId.Trim();
        ImageUrl = string.IsNullOrWhiteSpace(imageUrl) ? null : imageUrl.Trim();
        Attributes = NormalizeAttributes(attributes, "Product");
        IsActive = isActive;
        UpdatedAtUtc = utcNow;
    }

    internal static IReadOnlyDictionary<string, string> NormalizeAttributes(
        IReadOnlyDictionary<string, string>? attributes,
        string ownerName)
    {
        if (attributes is null || attributes.Count == 0)
        {
            return new Dictionary<string, string>();
        }

        var normalized = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var (key, value) in attributes)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"{ownerName} attribute names cannot be empty.", nameof(attributes));
            }

            var normalizedKey = key.Trim();
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{ownerName} attribute '{normalizedKey}' cannot be empty.", nameof(attributes));
            }

            normalized[normalizedKey] = value.Trim();
        }

        return normalized;
    }

    private static void Validate(string name, string description, string categoryId)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Product name is required.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Product description is required.", nameof(description));
        }

        if (string.IsNullOrWhiteSpace(categoryId))
        {
            throw new ArgumentException("Product category id is required.", nameof(categoryId));
        }
    }
}
