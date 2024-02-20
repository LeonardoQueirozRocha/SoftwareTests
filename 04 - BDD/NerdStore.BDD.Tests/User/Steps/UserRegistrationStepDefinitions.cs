using NerdStore.BDD.Tests.Configurations;
using TechTalk.SpecFlow;

namespace NerdStore.BDD.Tests.User.Steps;

[Binding]
[CollectionDefinition(nameof(AutomationWebFixtureCollection))]
public class UserRegistrationStepDefinitions
{
    private readonly AutomationWebTestsFixture _testsFixture;
    private readonly UserRegistrationScreen _userRegistrationScreen;

    public UserRegistrationStepDefinitions(AutomationWebTestsFixture testsFixture)
    {
        _testsFixture = testsFixture;
        _userRegistrationScreen = new UserRegistrationScreen(testsFixture.BrowserHelper);
    }

    [When(@"he clicks register")]
    public void WhenHeClicksRegister()
    {
        // Act
        _userRegistrationScreen.ClickOnRegistrationLink();

        // Assert
        Assert.Contains(_testsFixture.Configuration.RegisterUrl, _userRegistrationScreen.GetUrl());
    }

    [When(@"fill in the form data")]
    public void WhenFillInTheFormData(Table table)
    {
        // Arrange
        _testsFixture.GenerateUserData();
        var user = _testsFixture.User;

        // Act
        _userRegistrationScreen.FillRegisterForm(user);

        // Assert
        Assert.True(_userRegistrationScreen.ValidateFillRegisterForm(user));
    }

    [When(@"click on the register button")]
    public void WhenClickOnTheRegisterButton()
    {
        _userRegistrationScreen.ClickOnRegisterButton();
    }

    [When(@"fill in the form data with a password without capital letters")]
    public void WhenFillInTheFormDataWithAPasswordWithoutCapitalLetters(Table table)
    {
        throw new PendingStepException();
    }

    [Then(@"he will recieve an error message that the password must contain an uppercase letter")]
    public void ThenHeWillRecieveAnErrorMessageThatThePasswordMustContainAnUppercaseLetter()
    {
        throw new PendingStepException();
    }

    [When(@"fill in the form data with a password without special character")]
    public void WhenFillInTheFormDataWithAPasswordWithoutSpecialCharacter(Table table)
    {
        throw new PendingStepException();
    }

    [Then(@"he will recieve an error message that the password must contain a special caracter")]
    public void ThenHeWillRecieveAnErrorMessageThatThePasswordMustContainASpecialCaracter()
    {
        throw new PendingStepException();
    }
}
