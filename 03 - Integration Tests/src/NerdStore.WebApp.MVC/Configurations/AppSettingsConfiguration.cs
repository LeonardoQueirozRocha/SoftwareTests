namespace NerdStore.WebApp.MVC.Configurations;

public static class AppSettingsConfiguration
{
    public static void AddAppSettingsConfiguration(this WebApplicationBuilder builder)
    {
        builder.Configuration
            .SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
            .AddEnvironmentVariables();

        builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(nameof(AppSettings)));
    }
}