using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace NerdStore.BDD.Tests.Configurations;

public static class WebDriverFactory
{
    public static IWebDriver CreateWebDriver(Browser browser, string driverPath, bool headless)
    {
        var webDriver = browser switch
        {
            Browser.Firefox => ConfigureFirefox(driverPath, headless),
            Browser.Chrome => ConfigureChrome(driverPath, headless),
            _ => null
        };

        return webDriver;
    }

    private static IWebDriver ConfigureFirefox(string driverPath, bool headless)
    {
        var optionsFireFox = new FirefoxOptions();

        if (headless) optionsFireFox.AddArgument("--headless");

        var webDriver = new FirefoxDriver(driverPath, optionsFireFox);

        return webDriver;
    }

    private static IWebDriver ConfigureChrome(string driverPath, bool headless)
    {
        var options = new ChromeOptions();

        if (headless) options.AddArgument("--headless");

        var webDriver = new ChromeDriver(driverPath, options);

        return webDriver;
    }
}
