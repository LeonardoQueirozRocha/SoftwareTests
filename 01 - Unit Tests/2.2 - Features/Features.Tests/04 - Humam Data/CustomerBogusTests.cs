namespace Features.Tests.HumamData;

[Collection(nameof(CustomerBogusCollection))]
public class CustomerBogusTests
{
    private readonly CustomerTestsBogusFixture _customerTestsBogusFixture;

    public CustomerBogusTests(CustomerTestsBogusFixture customerTestsBogusFixture)
    {
        _customerTestsBogusFixture = customerTestsBogusFixture;
    }

    [Fact(DisplayName = "New Valid Customer")]
    [Trait("Category", "Customer Bogus Tests")]
    public void Customer_NewCustomer_ShouldBeValid()
    {
        // Arrange
        var customer = _customerTestsBogusFixture.BuildValidCustomer();

        // Act
        var result = customer.IsValid();

        // Assert
        Assert.True(result);
        Assert.Equal(0, customer.ValidationResult?.Errors.Count);
    }

    [Fact(DisplayName = "New Invalid Customer")]
    [Trait("Category", "Customer Bogus Tests")]
    public void Customer_NewCustomer_ShouldBeInvalid()
    {
        // Arrange
        var customer = _customerTestsBogusFixture.BuildInvalidCustomer();

        // Act
        var result = customer.IsValid();

        // Assert
        Assert.False(result);
        Assert.NotEqual(0, customer.ValidationResult?.Errors.Count);
    }
}