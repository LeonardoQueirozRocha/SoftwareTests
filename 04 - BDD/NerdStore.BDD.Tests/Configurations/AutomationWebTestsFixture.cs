namespace NerdStore.BDD.Tests.Configurations;

[CollectionDefinition(nameof(AutomationWebFixtureCollection))]
public class AutomationWebFixtureCollection : ICollectionFixture<AutomationWebTestsFixture> { }

public class AutomationWebTestsFixture
{
    public SeleniumHelper BrowserHelper;
    public readonly ConfigurationHelper Configuration;

    public AutomationWebTestsFixture()
    {
        Configuration = new ConfigurationHelper();
        BrowserHelper = new SeleniumHelper(Browser.Chrome, Configuration, false);
    }
}
