using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NerdStore.Catalog.Data;
using NerdStore.Sales.Data;
using NerdStore.WebApp.MVC.Data;

namespace NerdStore.WebApp.MVC.Configurations;

public static class DbContextConfiguration
{
    public static void AddDbContextConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."); ;

        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
        services.AddDbContext<CatalogContext>(options => options.UseSqlServer(connectionString));
        services.AddDbContext<SalesContext>(options => options.UseSqlServer(connectionString));
        services.AddDatabaseDeveloperPageExceptionFilter();
        services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>();
    }
}