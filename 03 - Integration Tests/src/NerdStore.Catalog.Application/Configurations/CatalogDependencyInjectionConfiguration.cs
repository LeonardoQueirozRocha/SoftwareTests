using Microsoft.Extensions.DependencyInjection;
using NerdStore.Catalog.Application.Interfaces.Services;
using NerdStore.Catalog.Application.Services;
using NerdStore.Catalog.Data;
using NerdStore.Catalog.Data.Repositories;
using NerdStore.Catalog.Domain.Interfaces.Repositories;
using NerdStore.Catalog.Domain.Interfaces.Services;
using NerdStore.Catalog.Domain.Services;

namespace NerdStore.Catalog.Application.Configurations;

public static class CatalogDependencyInjectionConfiguration
{
    public static void AddCatalogDependencies(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductAppService, ProductAppService>();
        services.AddScoped<IStockService, StockService>();
        services.AddScoped<CatalogContext>();
    }
}