using NerdStore.Catalog.Application.Configurations;
using NerdStore.Core.Configurations;
using NerdStore.Sales.Application.Configurations;

namespace NerdStore.WebApp.MVC.Configurations;

public static class DependencyInjectionConfiguration
{
    public static void AddServicesDependencies(this IServiceCollection services)
    {
        services.AddCoreDependencies();
        services.AddCatalogDependencies();
        services.AddSalesDependencies();
    }
}