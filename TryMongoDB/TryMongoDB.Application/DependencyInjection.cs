using Microsoft.Extensions.DependencyInjection;
using TryMongoDB.Application.Catalog.Categories;
using TryMongoDB.Application.Catalog.Products;

namespace TryMongoDB.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();

        return services;
    }
}
