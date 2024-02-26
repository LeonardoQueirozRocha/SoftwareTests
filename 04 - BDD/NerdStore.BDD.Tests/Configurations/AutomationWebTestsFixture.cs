using Bogus;

namespace NerdStore.BDD.Tests.Configurations;

[CollectionDefinition(nameof(AutomationWebFixtureCollection))]
public class AutomationWebFixtureCollection : ICollectionFixture<AutomationWebTestsFixture> { }

public class AutomationWebTestsFixture
{
    public SeleniumHelper BrowserHelper;
    public readonly ConfigurationHelper Configuration;
    public User.User User;

    public AutomationWebTestsFixture()
    {
        User = new User.User();
        Configuration = new ConfigurationHelper();
        BrowserHelper = new SeleniumHelper(Browser.Chrome, Configuration);
    }

    public void GenerateUserData()
    {
        var faker = new Faker("pt_BR");
        User.Email = faker.Internet.Email().ToLower();
        User.Password = faker.Internet.Password(8, false, "", "@1Ab_");
    }
}
