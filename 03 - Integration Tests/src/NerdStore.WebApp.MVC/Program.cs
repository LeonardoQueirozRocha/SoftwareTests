using NerdStore.WebApp.MVC.Configurations;

namespace NerdStore.WebApp.MVC;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddAppSettingsConfiguration();
        builder.Services.AddDbContextConfiguration(builder.Configuration);
        builder.Services.AddWebAppConfiguration();
        builder.Services.AddServicesDependencies();
        builder.Services.AddSwaggerConfiguration();

        var app = builder.Build();

        app.UseWebAppConfiguration(app.Environment);
        app.UseSwaggerConfiguration();

        app.Run();
    }
}