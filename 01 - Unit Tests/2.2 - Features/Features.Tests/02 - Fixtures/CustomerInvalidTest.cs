namespace Features.Tests.Fixtures;

[Collection(nameof(CustomerCollection))]
public class CustomerInvalidTest
{
    private readonly CustomerTestsFixture _customerTestsFixture;

    public CustomerInvalidTest(CustomerTestsFixture customerTestsFixture)
    {
        _customerTestsFixture = customerTestsFixture;
    }

    [Fact(DisplayName = "New Invalid Customer")]
    [Trait("Category", "Customer Fixture Tests")]
    public void Customer_NewCustomer_ShouldBeInvalid()
    {
        // Arrange
        var customer = _customerTestsFixture.BuildInvalidCustomer();

        // Act
        var result = customer.IsValid();

        // Assert
        Assert.False(result);
        Assert.NotEqual(0, customer.ValidationResult?.Errors.Count);
    }
}