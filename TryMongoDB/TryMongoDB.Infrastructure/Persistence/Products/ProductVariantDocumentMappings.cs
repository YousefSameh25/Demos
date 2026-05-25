using TryMongoDB.Domain.Products;

namespace TryMongoDB.Infrastructure.Persistence.Products;

internal static class ProductVariantDocumentMappings
{
    public static ProductVariant ToDomain(this ProductVariantDocument document) =>
        ProductVariant.Rehydrate(
            document.Id,
            document.ProductId,
            document.Sku,
            document.Price,
            document.Currency,
            document.StockQuantity,
            document.Attributes,
            document.IsActive);

    public static ProductVariantDocument ToDocument(this ProductVariant variant) =>
        new()
        {
            Id = variant.Id,
            ProductId = variant.ProductId,
            Sku = variant.Sku,
            Price = variant.Price,
            Currency = variant.Currency,
            StockQuantity = variant.StockQuantity,
            Attributes = variant.Attributes.ToDictionary(),
            IsActive = variant.IsActive
        };
}
