using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
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

    public bool ValidateUrlContent(string content)
    {
        return Wait.Until(ExpectedConditions.UrlContains(content));
    }

    public void ClickLinkByText(string linkText)
    {
        var link = GetElement(By.LinkText(linkText));
        link.Click();
    }

    public void ClickButtonById(string buttonId) 
    {
        var button = GetElement(By.Id(buttonId));
        button.Click();
    }

    public void ClickByXPath(string xPath)
    {
        var element = GetElement(By.XPath(xPath));
        element.Click();
    }

    public IWebElement GetElementByCssClass(string cssClass)
    {
        return GetElement(By.ClassName(cssClass));
    }

    public IWebElement GetElementByXPath(string xpath)
    {
        return GetElement(By.XPath(xpath));
    }

    public void FillTextBoxById(string fieldId, string fieldValue)
    {
        var field = GetElement(By.Id(fieldId));
        field.SendKeys(fieldValue);
    }

    public void FillDropDownById(string fieldId, string fieldValue)
    {
        var field = GetElement(By.Id(fieldId));
        var selectedelement = new SelectElement(field);
        selectedelement.SelectByValue(fieldValue);
    }

    public string GetTextElementByCssClass(string className)
    {
        return GetElement(By.ClassName(className)).Text;
    }

    public string GetTextElementById(string id)
    {
        return GetElement(By.Id(id)).Text;
    }

    public string GetTextBoxValuebyId(string id)
    {
        return GetElement(By.Id(id)).GetAttribute("value");
    }

    public IEnumerable<IWebElement> GetListByClassName(string className)
    {
        return GetElements(By.ClassName(className));
    }

    public bool ValidateIfElementExistsById(string id)
    {
        return ElementExists(By.Id(id));
    }

    public void BackNavigation(int times = 1)
    {
        for (int i = 0; i < times; i++) 
            WebDriver.Navigate().Back();
    }

    public void GetScreenshot(string name)
    {
        var screenshot = WebDriver.TakeScreenshot();
        var fileName = string.Format(string.Concat("{0}_", name, ".png"), DateTime.Now.ToFileTime());
        SaveScreenshot(screenshot, fileName);
    }

    public void SaveScreenshot(Screenshot screenshot, string fileName)
    {
        screenshot.SaveAsFile(string.Concat(Configuration.FolderPicture, fileName));
    }

    private IWebElement GetElement(By by)
    {
        return Wait.Until(ExpectedConditions.ElementIsVisible(by));
    }

    private IEnumerable<IWebElement> GetElements(By by)
    {
        return Wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(by));
    }

    private bool ElementExists(By by)
    {
        try
        {
            WebDriver.FindElement(by);
            return true;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }

    public void Dispose()
    {
        WebDriver.Quit();
        WebDriver.Dispose();
    }
}
