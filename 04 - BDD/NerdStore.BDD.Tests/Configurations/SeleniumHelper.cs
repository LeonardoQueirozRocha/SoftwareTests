using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace NerdStore.BDD.Tests.Configurations;

public class SeleniumHelper : IDisposable
{
    public IWebDriver WebDriver;
    public readonly ConfigurationHelper Configuration;
    public WebDriverWait Wait;

    public SeleniumHelper(
        Browser browser, 
        ConfigurationHelper configuration, 
        bool headless = true)
    {
        Configuration = configuration;
        WebDriver = WebDriverFactory.CreateWebDriver(browser, Configuration.WebDrivers, headless);
        WebDriver.Manage().Window.Maximize();
        Wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(30));
    }

    public string GetUrl()
    {
        return WebDriver.Url;
    }

    public void GoToUrl(string url)
    {
        WebDriver.Navigate().GoToUrl(url);
    }

    public void ClickLinkByText(string linkText)
    {
        Wait
            .Until(ExpectedConditions.ElementIsVisible(By.LinkText(linkText)))
            .Click();
    }


    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
