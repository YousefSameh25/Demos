namespace TryMongoDB.Infrastructure.Persistence;

public sealed class MongoDbSettings
{
    public const string SectionName = "MongoDb";

    public string ConnectionString { get; init; } = "mongodb://localhost:27017";
    public string DatabaseName { get; init; } = "try_mongo_db";
    public string ProductsCollectionName { get; init; } = "products";
    public string ProductVariantsCollectionName { get; init; } = "productVariants";
    public string CategoriesCollectionName { get; init; } = "categories";
}
