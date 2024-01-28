using System.Globalization;
using System.Reflection;
using NerdStore.Catalog.Application.AutoMapper;

namespace NerdStore.WebApp.MVC.Configurations;

public static class WebAppConfiguration
{
    public static void AddWebAppConfiguration(this IServiceCollection services)
    {
        services.AddControllersWithViews();
        services.AddHttpContextAccessor();

        services.AddAutoMapper(
            typeof(DomainToViewModelMappingProfile),
            typeof(ViewModelToDomainMappingProfile));

        services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddEndpointsApiExplorer();
    }

    public static void UseWebAppConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        var cultureInfo = new CultureInfo("en-US");
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();
        app.UseAuthorization();

        app.UseEndpoints(endpoint =>
        {
            endpoint.MapControllerRoute("default", "{controller=Showcase}/{action=Index}/{id?}");
            endpoint.MapRazorPages();
        });

    }
}