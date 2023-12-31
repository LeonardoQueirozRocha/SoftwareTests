namespace Features.Tests.Fixtures;

[Collection(nameof(CustomerCollection))]
public class CustomerValidTests
{
    private readonly CustomerTestsFixture _customerTestsFixture;

    public CustomerValidTests(CustomerTestsFixture customerTestsFixture)
    {
        _customerTestsFixture = customerTestsFixture;
    }

    [Fact(DisplayName = "New Valid Customer")]
    [Trait("Category", "Customer Fixture Tests")]
    public void Customer_NewCustomer_ShouldBeValid()
    {
        // Arrange
        var customer = _customerTestsFixture.BuildValidCustomer();

        // Act
        var result = customer.IsValid();

        // Assert
        Assert.True(result);
        Assert.Equal(0, customer.ValidationResult?.Errors.Count);
    }
}