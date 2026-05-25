namespace TryMongoDB.Domain.Products;

public sealed class ProductVariant
{
    private ProductVariant(
        string id,
        string productId,
        string sku,
        decimal price,
        string currency,
        int stockQuantity,
        IReadOnlyDictionary<string, string> attributes,
        bool isActive)
    {
        Id = id;
        ProductId = productId;
        Sku = sku;
        Price = price;
        Currency = currency;
        StockQuantity = stockQuantity;
        Attributes = attributes;
        IsActive = isActive;
    }

    public string Id { get; }
    public string ProductId { get; }
    public string Sku { get; }
    public decimal Price { get; private set; }
    public string Currency { get; private set; }
    public int StockQuantity { get; private set; }
    public IReadOnlyDictionary<string, string> Attributes { get; private set; }
    public bool IsActive { get; private set; }

    public static ProductVariant Create(
        string productId,
        string sku,
        decimal price,
        string currency,
        int stockQuantity,
        IReadOnlyDictionary<string, string>? attributes,
        bool isActive = true) =>
        Rehydrate(Guid.NewGuid().ToString("N"), productId, sku, price, currency, stockQuantity, attributes, isActive);

    public static ProductVariant Rehydrate(
        string id,
        string productId,
        string sku,
        decimal price,
        string currency,
        int stockQuantity,
        IReadOnlyDictionary<string, string>? attributes,
        bool isActive)
    {
        Validate(id, productId, sku, price, currency, stockQuantity);

        return new ProductVariant(
            id.Trim(),
            productId.Trim(),
            sku.Trim(),
            price,
            currency.Trim().ToUpperInvariant(),
            stockQuantity,
            Product.NormalizeAttributes(attributes, "Product variant"),
            isActive);
    }

    private static void Validate(string id, string productId, string sku, decimal price, string currency, int stockQuantity)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("Product variant id is required.", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(productId))
        {
            throw new ArgumentException("Product variant product id is required.", nameof(productId));
        }

        if (string.IsNullOrWhiteSpace(sku))
        {
            throw new ArgumentException("Product variant SKU is required.", nameof(sku));
        }

        if (price < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Product variant price cannot be negative.");
        }

        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new ArgumentException("Product variant currency is required.", nameof(currency));
        }

        if (stockQuantity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(stockQuantity), "Product variant stock quantity cannot be negative.");
        }
    }
}
