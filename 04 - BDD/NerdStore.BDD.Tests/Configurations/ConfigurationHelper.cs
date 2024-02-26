using Microsoft.Extensions.Configuration;

namespace NerdStore.BDD.Tests.Configurations;

public class ConfigurationHelper
{
    private readonly IConfiguration _configuration;

    public ConfigurationHelper()
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
    }

    public string WebDrivers => GetValue(nameof(WebDrivers));
    public string DomainUrl => GetValue(nameof(DomainUrl));
    public string ShowcaseUrl => GetValue(nameof(ShowcaseUrl));
    public string ProductUrl => string.Concat(DomainUrl, GetValue(nameof(ProductUrl)));
    public string CartUrl => string.Concat(DomainUrl, GetValue(nameof(CartUrl)));
    public string RegisterUrl => string.Concat(DomainUrl, GetValue(nameof(RegisterUrl)));
    public string LoginUrl => string.Concat(DomainUrl, GetValue(nameof(LoginUrl)));
    public string FolderPath => Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));
    public string FolderPicture => string.Concat(FolderPath, GetValue(nameof(FolderPicture)));

    private string GetValue(string key) => _configuration.GetSection(key).Value;
}
