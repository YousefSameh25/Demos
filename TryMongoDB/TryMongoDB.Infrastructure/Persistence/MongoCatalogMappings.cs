using MongoDB.Bson.Serialization;
using TryMongoDB.Infrastructure.Persistence.Categories;
using TryMongoDB.Infrastructure.Persistence.Products;

namespace TryMongoDB.Infrastructure.Persistence;

internal static class MongoCatalogMappings
{
    public static void Register()
    {
        RegisterProduct();
        RegisterCategory();
    }

    private static void RegisterProduct()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(ProductVariantDocument)))
        {
            BsonClassMap.RegisterClassMap<ProductVariantDocument>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapIdMember(variant => variant.Id);
                map.MapMember(variant => variant.ProductId).SetElementName("productId");
                map.MapMember(variant => variant.Sku).SetElementName("sku");
                map.MapMember(variant => variant.Price).SetElementName("price");
                map.MapMember(variant => variant.Currency).SetElementName("currency");
                map.MapMember(variant => variant.StockQuantity).SetElementName("stockQuantity");
                map.MapMember(variant => variant.Attributes).SetElementName("attributes");
                map.MapMember(variant => variant.IsActive).SetElementName("isActive");
            });
        }

        if (BsonClassMap.IsClassMapRegistered(typeof(ProductDocument)))
        {
            return;
        }

        BsonClassMap.RegisterClassMap<ProductDocument>(map =>
        {
            map.AutoMap();
            map.SetIgnoreExtraElements(true);
            map.MapIdMember(product => product.Id);
            map.MapMember(product => product.Name).SetElementName("name");
            map.MapMember(product => product.Description).SetElementName("description");
            map.MapMember(product => product.CategoryId).SetElementName("categoryId");
            map.MapMember(product => product.ImageUrl).SetElementName("imageUrl");
            map.MapMember(product => product.Attributes).SetElementName("attributes");
            map.MapMember(product => product.IsActive).SetElementName("isActive");
            map.MapMember(product => product.CreatedAtUtc).SetElementName("createdAtUtc");
            map.MapMember(product => product.UpdatedAtUtc).SetElementName("updatedAtUtc");
        });
    }

    private static void RegisterCategory()
    {
        if (BsonClassMap.IsClassMapRegistered(typeof(CategoryDocument)))
        {
            return;
        }

        BsonClassMap.RegisterClassMap<CategoryDocument>(map =>
        {
            map.AutoMap();
            map.SetIgnoreExtraElements(true);
            map.MapIdMember(category => category.Id);
            map.MapMember(category => category.Name).SetElementName("name");
            map.MapMember(category => category.Slug).SetElementName("slug");
            map.MapMember(category => category.Description).SetElementName("description");
            map.MapMember(category => category.IsActive).SetElementName("isActive");
            map.MapMember(category => category.CreatedAtUtc).SetElementName("createdAtUtc");
            map.MapMember(category => category.UpdatedAtUtc).SetElementName("updatedAtUtc");
        });
    }
}
