using Features.Custumers.Models;

namespace Features.Tests.Traits;

public class CustomerTests
{
    [Fact(DisplayName = "New Valid Customer")]
    [Trait("Category", "Customer Trait Tests")]
    public void Customer_NewCustomer_ShouldBeValid()
    {
        // Arrange
        var customer = new Customer(
            id: Guid.NewGuid(),
            name: "Leonardo",
            lastName: "Rocha",
            birthDate: DateTime.Now.AddYears(-25),
            registrationDate: DateTime.Now,
            email: "leo@leo.com",
            active: true
        );

        // Act
        var result = customer.IsValid();

        // Assert
        Assert.True(result);
        Assert.Equal(0, customer.ValidationResult?.Errors.Count);
    }

    [Fact(DisplayName = "New Invalid Customer")]
    [Trait("Category", "Customer Trait Tests")]
    public void Customer_NewCustomer_ShouldBeInvalid()
    {
        // Arrange
        var customer = new Customer(
            id: Guid.NewGuid(),
            name: string.Empty,
            lastName: string.Empty,
            birthDate: DateTime.Now,
            registrationDate: DateTime.Now,
            email: "leo2@leo.com",
            active: true
        );

        // Act
        var result = customer.IsValid();

        // Assert
        Assert.False(result);
        Assert.NotEqual(0, customer.ValidationResult?.Errors.Count);
    }
}