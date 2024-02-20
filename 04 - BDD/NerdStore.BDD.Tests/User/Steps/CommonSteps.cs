using NerdStore.BDD.Tests.Configurations;
using TechTalk.SpecFlow;

namespace NerdStore.BDD.Tests.User.Steps;

[Binding]
[CollectionDefinition(nameof(AutomationWebFixtureCollection))]
public class CommonSteps
{
    private readonly AutomationWebTestsFixture _testsFixture;
    private readonly UserRegistrationScreen _userRegistrationScreen;

    public CommonSteps(AutomationWebTestsFixture testsFixture)
    {
        _testsFixture = testsFixture;
        _userRegistrationScreen = new UserRegistrationScreen(testsFixture.BrowserHelper);
    }

    [Given(@"that the visitor is accessing the store's website")]
    public void GivenThatTheVisitorIsAccessingTheStoresWebsite()
    {
        // Act
        _userRegistrationScreen.AccessStoreWebSite();

        // Assert
        Assert.Contains(_testsFixture.Configuration.DomainUrl, _userRegistrationScreen.GetUrl());
    }

    [Then(@"a greeting with his email will be displayed in the top menu")]
    public void ThenAGreetingWithHisEmailWillBeDisplayedInTheTopMenu()
    {
        // Assert
        Assert.True(_userRegistrationScreen.ValidateLoggedUserGreeting(_testsFixture.User));
    }

    [Then(@"it will be redirected to the storefront")]
    public void ThenItWillBeRedirectedToTheStorefront()
    {
        // Assert
        Assert.Equal(_testsFixture.Configuration.ShowcaseUrl, _userRegistrationScreen.GetUrl());
    }
}
