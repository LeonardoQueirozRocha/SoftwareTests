using NerdStore.BDD.Tests.Configurations;
using TechTalk.SpecFlow;

namespace NerdStore.BDD.Tests.User.Steps;

[Binding]
[CollectionDefinition(nameof(AutomationWebFixtureCollection))]
public class UserLoginStepDefinitions
{
    private readonly UserLoginScreen _userLoginScreen;
    private readonly AutomationWebTestsFixture _testsFixture;

    public UserLoginStepDefinitions(AutomationWebTestsFixture testsFixture)
    {
        _testsFixture = testsFixture;
        _userLoginScreen = new UserLoginScreen(testsFixture.BrowserHelper);
    }

    [When(@"he clicks login")]
    public void WhenHeClicksLogin()
    {
        // Act
        _userLoginScreen.ClickOnLoginLink();

        // Assert
        Assert.Contains(_testsFixture.Configuration.LoginUrl, _userLoginScreen.GetUrl());
    }

    [When(@"fill in the login form data")]
    public void WhenFillInTheLoginFormData(Table table)
    {
        // Arrange
        var user = new User
        {
            Email = "teste@teste.com",
            Password = "Teste@123"
        };

        _testsFixture.User = user;

        // Act
        _userLoginScreen.FillLoginForm(user);

        // Assert
        Assert.True(_userLoginScreen.ValidateFilledLoginForm(user));
    }

    [When(@"click on the login button")]
    public void WhenClickOnTheLoginButton()
    {
        _userLoginScreen.ClickOnLoginButton();
    }
}
