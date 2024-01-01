using Features.Tests.AutoMock;
using FluentAssertions;

namespace Features.Tests.HumamData;

[Collection(nameof(CustomerAutoMockerCollection))]
public class CustomerFluentAssertionTests
{
    private readonly CustomerTestsAutoMockerFixture _customerTestsAutoMockerFixture;

    public CustomerFluentAssertionTests(CustomerTestsAutoMockerFixture customerTestsAutoMockerFixture)
    {
        _customerTestsAutoMockerFixture = customerTestsAutoMockerFixture;
    }

    [Fact(DisplayName = "New Valid Customer")]
    [Trait("Category", "Customer Fluent Assertion Tests")]
    public void Customer_NewCustomer_ShouldBeValid()
    {
        // Arrange
        var customer = _customerTestsAutoMockerFixture.BuildValidCustomer();

        // Act
        var result = customer.IsValid();

        // Assert
        result.Should().BeTrue();
        customer.ValidationResult.Errors.Should().HaveCount(0);
    }

    [Fact(DisplayName = "New Invalid Customer")]
    [Trait("Category", "Customer Fluent Assertion Tests")]
    public void Customer_NewCustomer_ShouldBeInvalid()
    {
        // Arrange
        var customer = _customerTestsAutoMockerFixture.BuildInvalidCustomer();

        // Act
        var result = customer.IsValid();

        // Assert
        result.Should().BeFalse();
        customer.ValidationResult.Errors.Should().HaveCountGreaterThanOrEqualTo(1, "should contains validations errors");
    }
}