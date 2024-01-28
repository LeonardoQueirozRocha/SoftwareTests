using Microsoft.Extensions.DependencyInjection;
using NerdStore.Sales.Application.Interfaces;
using NerdStore.Sales.Application.Queries;
using NerdStore.Sales.Data;
using NerdStore.Sales.Data.Repositories;
using NerdStore.Sales.Domain.Interfaces.Repositories;

namespace NerdStore.Sales.Application.Configurations;

public static class SalesDependencyInjectionConfiguration
{
    public static void AddSalesDependencies(this IServiceCollection services)
    {
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderQueries, OrderQueries>();
        services.AddScoped<SalesContext>();
    }
}