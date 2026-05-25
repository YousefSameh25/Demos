using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TryMongoDB.Application.Catalog.Categories;
using TryMongoDB.Application.Catalog.Products;
using TryMongoDB.Infrastructure.Persistence;
using TryMongoDB.Infrastructure.Persistence.Categories;
using TryMongoDB.Infrastructure.Persistence.Products;

namespace TryMongoDB.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        MongoCatalogMappings.Register();

        services.Configure<MongoDbSettings>(configuration.GetSection(MongoDbSettings.SectionName));
        services.AddSingleton<MongoCatalogContext>();
        services.AddScoped<IProductRepository, MongoProductRepository>();
        services.AddScoped<IProductVariantRepository, MongoProductVariantRepository>();
        services.AddScoped<ICategoryRepository, MongoCategoryRepository>();

        return services;
    }
}
