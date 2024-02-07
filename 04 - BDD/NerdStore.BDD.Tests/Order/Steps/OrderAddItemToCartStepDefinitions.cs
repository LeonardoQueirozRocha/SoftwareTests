using TechTalk.SpecFlow;

namespace NerdStore.BDD.Tests.Order.Steps;

[Binding]
public class OrderAddItemToCartStepDefinitions
{
    [Given(@"the user is logged in")]
    public void GivenTheUserIsLoggedIn()
    {
        throw new PendingStepException();
    }

    [Given(@"that a product is in the window")]
    public void GivenThatAProductIsInTheWindow()
    {
        throw new PendingStepException();
    }

    [Given(@"be available in stock")]
    public void GivenBeAvailableInStock()
    {
        throw new PendingStepException();
    }

    [Given(@"do not have any products added to the cart")]
    public void GivenDoNotHaveAnyProductsAddedToTheCart()
    {
        throw new PendingStepException();
    }

    [When(@"the user adds a unit to the cart")]
    public void WhenTheUserAddsAUnitToTheCart()
    {
        throw new PendingStepException();
    }

    [Then(@"the user will be redirected to the purchase summary")]
    public void ThenTheUserWillBeRedirectedToThePurchaseSummary()
    {
        throw new PendingStepException();
    }

    [Then(@"the total order value will be exactly the value of the added item")]
    public void ThenTheTotalOrderValueWillBeExactlyTheValueOfTheAddedItem()
    {
        throw new PendingStepException();
    }

    [When(@"the user adds an item above the maximum quantity allowed")]
    public void WhenTheUserAddsAnItemAboveTheMaximumQuantityAllowed()
    {
        throw new PendingStepException();
    }

    [Then(@"receive an error message stating that the limit quantity has been exceeded")]
    public void ThenReceiveAnErrorMessageStatingThatTheLimitQuantityHasBeenExceeded()
    {
        throw new PendingStepException();
    }

    [Given(@"the same product has already been added to the cart previously")]
    public void GivenTheSameProductHasAlreadyBeenAddedToTheCartPreviously()
    {
        throw new PendingStepException();
    }

    [Then(@"the quantity of items for that product will have been increased by one more unit")]
    public void ThenTheQuantityOfItemsForThatProductWillHaveBeenIncreasedByOneMoreUnit()
    {
        throw new PendingStepException();
    }

    [Then(@"the total order value will be the multiplication of the quantity of items by the unit value")]
    public void ThenTheTotalOrderValueWillBeTheMultiplicationOfTheQuantityOfItemsByTheUnitValue()
    {
        throw new PendingStepException();
    }

    [When(@"the user adds the maximum quantity allowed to the cart")]
    public void WhenTheUserAddsTheMaximumQuantityAllowedToTheCart()
    {
        throw new PendingStepException();
    }
}
