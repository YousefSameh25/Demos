using MongoDB.Driver;
using TryMongoDB.Application.Catalog.Products;
using TryMongoDB.Domain.Products;

namespace TryMongoDB.Infrastructure.Persistence.Products;

public sealed class MongoProductRepository(MongoCatalogContext context) : IProductRepository
{
    public async Task<Product?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var document = await context.Products
            .Find(product => product.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        return document?.ToDomain();
    }

    public async Task<IReadOnlyCollection<Product>> ListAsync(ProductFilter filter, CancellationToken cancellationToken)
    {
        var builder = Builders<ProductDocument>.Filter;
        var filters = new List<FilterDefinition<ProductDocument>>();

        if (!string.IsNullOrWhiteSpace(filter.CategoryId))
        {
            filters.Add(builder.Eq(product => product.CategoryId, filter.CategoryId));
        }

        if (filter.IsActive is not null)
        {
            filters.Add(builder.Eq(product => product.IsActive, filter.IsActive.Value));
        }

        if (filter.ProductIds is { Count: > 0 })
        {
            filters.Add(builder.In(product => product.Id, filter.ProductIds));
        }

        if (!string.IsNullOrWhiteSpace(filter.SearchText))
        {
            var search = filter.SearchText.Trim();
            filters.Add(builder.Or(
                builder.Regex(product => product.Name, new MongoDB.Bson.BsonRegularExpression(search, "i")),
                builder.Regex(product => product.Description, new MongoDB.Bson.BsonRegularExpression(search, "i"))));
        }

        var mongoFilter = filters.Count == 0 ? builder.Empty : builder.And(filters);
        var documents = await context.Products
            .Find(mongoFilter)
            .SortBy(product => product.Name)
            .ToListAsync(cancellationToken);

        return documents.Select(product => product.ToDomain()).ToArray();
    }

    public Task AddAsync(Product product, CancellationToken cancellationToken) =>
        context.Products.InsertOneAsync(product.ToDocument(), cancellationToken: cancellationToken);

    public Task UpdateAsync(Product product, CancellationToken cancellationToken) =>
        context.Products.ReplaceOneAsync(
            existing => existing.Id == product.Id,
            product.ToDocument(),
            cancellationToken: cancellationToken);

    public Task DeleteAsync(string id, CancellationToken cancellationToken) =>
        context.Products.DeleteOneAsync(product => product.Id == id, cancellationToken);
}
