using NerdStore.BDD.Tests.Configurations;
using TechTalk.SpecFlow;

namespace NerdStore.BDD.Tests.Order.Steps;

[Binding]
[CollectionDefinition(nameof(AutomationWebFixtureCollection))]
public class OrderAddItemToCartStepDefinitions
{
    private readonly AutomationWebTestsFixture _testsFixture;
    private readonly OrderScreen _orderScreen;
    private string _productUrl;

    public OrderAddItemToCartStepDefinitions(AutomationWebTestsFixture testsFixture)
    {
        _testsFixture = testsFixture;
        _orderScreen = new OrderScreen(testsFixture.BrowserHelper);
    }

    [Given(@"the user is logged in")]
    public void GivenTheUserIsLoggedIn()
    {
        // Arrange

        // Act

        // Assert
    }

    [Given(@"that a product is in the window")]
    public void GivenThatAProductIsInTheWindow()
    {
        // Arrange
        _orderScreen.AccessProductShowcase();

        // Act
        _orderScreen.GetProductDetail();
        _productUrl = _orderScreen.GetUrl();

        // Assert
        Assert.True(_orderScreen.ValidateAvailableProduct());
    }

    [Given(@"be available in stock")]
    public void GivenBeAvailableInStock()
    {
        // Arrange

        // Act

        // Assert
    }

    [Given(@"do not have any products added to the cart")]
    public void GivenDoNotHaveAnyProductsAddedToTheCart()
    {
        // Arrange

        // Act

        // Assert
    }

    [When(@"the user adds a unit to the cart")]
    public void WhenTheUserAddsAUnitToTheCart()
    {
        // Arrange

        // Act

        // Assert
    }

    [Then(@"the user will be redirected to the purchase summary")]
    public void ThenTheUserWillBeRedirectedToThePurchaseSummary()
    {
        // Arrange

        // Act

        // Assert
    }

    [Then(@"the total order value will be exactly the value of the added item")]
    public void ThenTheTotalOrderValueWillBeExactlyTheValueOfTheAddedItem()
    {
        // Arrange

        // Act

        // Assert
    }

    [When(@"the user adds an item above the maximum quantity allowed")]
    public void WhenTheUserAddsAnItemAboveTheMaximumQuantityAllowed()
    {
        // Arrange

        // Act

        // Assert
    }

    [Then(@"receive an error message stating that the limit quantity has been exceeded")]
    public void ThenReceiveAnErrorMessageStatingThatTheLimitQuantityHasBeenExceeded()
    {
        // Arrange

        // Act

        // Assert
    }

    [Given(@"the same product has already been added to the cart previously")]
    public void GivenTheSameProductHasAlreadyBeenAddedToTheCartPreviously()
    {
        // Arrange

        // Act

        // Assert
    }

    [Then(@"the quantity of items for that product will have been increased by one more unit")]
    public void ThenTheQuantityOfItemsForThatProductWillHaveBeenIncreasedByOneMoreUnit()
    {
        // Arrange

        // Act

        // Assert
    }

    [Then(@"the total order value will be the multiplication of the quantity of items by the unit value")]
    public void ThenTheTotalOrderValueWillBeTheMultiplicationOfTheQuantityOfItemsByTheUnitValue()
    {
        // Arrange

        // Act

        // Assert
    }

    [When(@"the user adds the maximum quantity allowed to the cart")]
    public void WhenTheUserAddsTheMaximumQuantityAllowedToTheCart()
    {
        // Arrange

        // Act

        // Assert
    }
}
