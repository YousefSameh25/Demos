using TryMongoDB.Domain.Products;

namespace TryMongoDB.Infrastructure.Persistence.Products;

internal static class ProductDocumentMappings
{
    public static Product ToDomain(this ProductDocument document) =>
        Product.Rehydrate(
            document.Id,
            document.Name,
            document.Description,
            document.CategoryId,
            document.ImageUrl,
            document.Attributes,
            document.IsActive,
            document.CreatedAtUtc,
            document.UpdatedAtUtc);

    public static ProductDocument ToDocument(this Product product) =>
        new()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            CategoryId = product.CategoryId,
            ImageUrl = product.ImageUrl,
            Attributes = product.Attributes.ToDictionary(),
            IsActive = product.IsActive,
            CreatedAtUtc = product.CreatedAtUtc,
            UpdatedAtUtc = product.UpdatedAtUtc
        };
}
