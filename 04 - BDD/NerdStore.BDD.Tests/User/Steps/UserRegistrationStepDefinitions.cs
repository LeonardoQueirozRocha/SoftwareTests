using TechTalk.SpecFlow;

namespace NerdStore.BDD.Tests.User.Steps;

[Binding]
public class UserRegistrationStepDefinitions
{


    [When(@"he clicks register")]
    public void WhenHeClicksRegister()
    {
        throw new PendingStepException();
    }

    [When(@"fill in the form data")]
    public void WhenFillInTheFormData(Table table)
    {
        throw new PendingStepException();
    }

    [When(@"click on the register button")]
    public void WhenClickOnTheRegisterButton()
    {
        throw new PendingStepException();
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
