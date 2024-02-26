using NerdStore.BDD.Tests.Configurations;
using NerdStore.BDD.Tests.User;
using TechTalk.SpecFlow;

namespace NerdStore.BDD.Tests.Order.Steps;

[Binding]
[CollectionDefinition(nameof(AutomationWebFixtureCollection))]
public class OrderAddItemToCartStepDefinitions
{
    private readonly AutomationWebTestsFixture _testsFixture;
    private readonly OrderScreen _orderScreen;
    private readonly UserLoginScreen _userLoginScreen;
    private string _productUrl;

    public OrderAddItemToCartStepDefinitions(AutomationWebTestsFixture testsFixture)
    {
        _testsFixture = testsFixture;
        _orderScreen = new OrderScreen(testsFixture.BrowserHelper);
        _userLoginScreen = new UserLoginScreen(testsFixture.BrowserHelper);
    }

    [Given(@"the user is logged in")]
    public void GivenTheUserIsLoggedIn()
    {
        // Arrange
        var user = new User.User
        {
            Email = "teste@teste.com",
            Password = "Teste@123"
        };

        _testsFixture.User = user;

        // Act
        var login = _userLoginScreen.Login(user);

        // Assert
        Assert.True(login);
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
        // Assert
        Assert.True(_orderScreen.GetStockQuantity() > 0);
    }

    [Given(@"do not have any products added to the cart")]
    public void GivenDoNotHaveAnyProductsAddedToTheCart()
    {
        // Act
        _orderScreen.GoToShippingCart();
        _orderScreen.ResetShoppingCart();

        // Assert
        Assert.Equal(0, _orderScreen.GetCartTotalValue());

        _orderScreen.GoToUrl(_productUrl);
    }

    [When(@"the user adds a unit to the cart")]
    public void WhenTheUserAddsAUnitToTheCart()
    {
        // Act
        _orderScreen.ClickOnBuyNow();
    }

    [Then(@"the user will be redirected to the purchase summary")]
    public void ThenTheUserWillBeRedirectedToThePurchaseSummary()
    {
        // Assert
        Assert.True(_orderScreen.ValidateIsInShppingCart());
    }

    [Then(@"the total order value will be exactly the value of the added item")]
    public void ThenTheTotalOrderValueWillBeExactlyTheValueOfTheAddedItem()
    {
        // Arrange
        var unitValue = _orderScreen.GetProductCartUnitValue();
        var cartValue = _orderScreen.GetCartTotalValue();

        // Assert
        Assert.Equal(unitValue, cartValue);
    }

    [When(@"the user adds an item above the maximum quantity allowed")]
    public void WhenTheUserAddsAnItemAboveTheMaximumQuantityAllowed()
    {
        // Arrange
        _orderScreen.ClickOnAddItemsQuantity(Sales.Domain.Models.Order.MAX_UNITS_ITEM + 1);

        // Act
        _orderScreen.ClickOnBuyNow();
    }

    [Then(@"receive an error message stating that the limit quantity has been exceeded")]
    public void ThenReceiveAnErrorMessageStatingThatTheLimitQuantityHasBeenExceeded()
    {
        // Arrange
        var message = _orderScreen.GetProductErrorMessage();

        // Assert
        Assert.Contains($"Max quantity of an item is {Sales.Domain.Models.Order.MAX_UNITS_ITEM}", message);
    }

    [Given(@"the same product has already been added to the cart previously")]
    public void GivenTheSameProductHasAlreadyBeenAddedToTheCartPreviously()
    {
        // Act
        _orderScreen.GoToShippingCart();
        _orderScreen.ResetShoppingCart();
        _orderScreen.AccessProductShowcase();
        _orderScreen.GetProductDetail();
        _orderScreen.ClickOnBuyNow();

        // Assert
        Assert.True(_orderScreen.ValidateIsInShppingCart());

        _orderScreen.BackNavigation();
    }

    [Then(@"the quantity of items for that product will have been increased by one more unit")]
    public void ThenTheQuantityOfItemsForThatProductWillHaveBeenIncreasedByOneMoreUnit()
    {
        // Assert
        Assert.True(_orderScreen.GetFirtCartProductItemsQuantity() == 2);
    }

    [Then(@"the total order value will be the multiplication of the quantity of items by the unit value")]
    public void ThenTheTotalOrderValueWillBeTheMultiplicationOfTheQuantityOfItemsByTheUnitValue()
    {
        // Arrange
        var unitValue = _orderScreen.GetProductCartUnitValue();
        var cartValue = _orderScreen.GetCartTotalValue();
        var unitsQuantity = _orderScreen.GetFirtCartProductItemsQuantity();

        // Assert
        Assert.Equal(unitValue * unitsQuantity, cartValue);
    }

    [When(@"the user adds the maximum quantity allowed to the cart")]
    public void WhenTheUserAddsTheMaximumQuantityAllowedToTheCart()
    {
        // Arrange
        _orderScreen.ClickOnAddItemsQuantity(Sales.Domain.Models.Order.MAX_UNITS_ITEM);

        // Act
        _orderScreen.ClickOnBuyNow();
    }
}
