using TechTalk.SpecFlow;

namespace NerdStore.BDD.Tests.User.Steps;

[Binding]
public class CommonSteps
{
    [Given(@"that the visitor is accessing the store's website")]
    public void GivenThatTheVisitorIsAccessingTheStoresWebsite()
    {
        throw new PendingStepException();
    }

    [Then(@"a greeting with his email will be displayed in the top menu")]
    public void ThenAGreetingWithHisEmailWillBeDisplayedInTheTopMenu()
    {
        throw new PendingStepException();
    }

    [Then(@"it will be redirected to the storefront")]
    public void ThenItWillBeRedirectedToTheStorefront()
    {
        throw new PendingStepException();
    }
}
