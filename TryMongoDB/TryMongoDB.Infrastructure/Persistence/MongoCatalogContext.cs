using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TryMongoDB.Infrastructure.Persistence.Categories;
using TryMongoDB.Infrastructure.Persistence.Products;

namespace TryMongoDB.Infrastructure.Persistence;

public sealed class MongoCatalogContext
{
    public MongoCatalogContext(IOptions<MongoDbSettings> options)
    {
        var settings = options.Value;
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);

        Products = database.GetCollection<ProductDocument>(settings.ProductsCollectionName);
        ProductVariants = database.GetCollection<ProductVariantDocument>(settings.ProductVariantsCollectionName);
        Categories = database.GetCollection<CategoryDocument>(settings.CategoriesCollectionName);

        EnsureIndexes();
    }

    public IMongoCollection<ProductDocument> Products { get; }
    public IMongoCollection<ProductVariantDocument> ProductVariants { get; }
    public IMongoCollection<CategoryDocument> Categories { get; }

    private void EnsureIndexes()
    {
        ProductVariants.Indexes.CreateOne(new CreateIndexModel<ProductVariantDocument>(
            Builders<ProductVariantDocument>.IndexKeys.Ascending(variant => variant.Sku),
            new CreateIndexOptions { Unique = true }));

        ProductVariants.Indexes.CreateOne(new CreateIndexModel<ProductVariantDocument>(
            Builders<ProductVariantDocument>.IndexKeys.Ascending(variant => variant.ProductId)));

        Categories.Indexes.CreateOne(new CreateIndexModel<CategoryDocument>(
            Builders<CategoryDocument>.IndexKeys.Ascending(category => category.Slug),
            new CreateIndexOptions { Unique = true }));
    }
}
