using NerdStore.Sales.Application.Commands;
using NerdStore.Sales.Application.Commands.Validators;
using NerdStore.Sales.Domain.Models;

namespace NerdStore.Sales.Application.Tests.Orders;

public class AddOrderItemCommandTests
{
    [Fact(DisplayName = "Add valid order item command")]
    [Trait("Category", "Sales - Order Commands")]
    public void AddOrderItemCommand_CommandIsValid_ShouldPassInTheValidation()
    {
        // Arrange
        var orderCommand = new AddOrderItemCommand(Guid.NewGuid(), Guid.NewGuid(), "Test Product", 2, 10);

        // Act
        var result = orderCommand.IsValid();

        // Assert
        Assert.True(result);
        Assert.Empty(orderCommand.ValidationResult.Errors);
    }

    [Fact(DisplayName = "Add invalid order item command")]
    [Trait("Category", "Sales - Order Commands")]
    public void AddOrderItemCommand_CommandIsInvalid_ShouldNotPassInTheValidation()
    {
        // Arrange
        var orderCommand = new AddOrderItemCommand(Guid.Empty, Guid.Empty, string.Empty, 0, 0);

        // Act
        var result = orderCommand.IsValid();

        // Assert
        Assert.False(result);
        Assert.NotEmpty(orderCommand.ValidationResult.Errors);
        Assert.Contains(AddOrderItemValidator.CustomerIdErrorMessage, orderCommand.ValidationResult.Errors.Select(x => x.ErrorMessage));
        Assert.Contains(AddOrderItemValidator.ProductIdErrorMessage, orderCommand.ValidationResult.Errors.Select(x => x.ErrorMessage));
        Assert.Contains(AddOrderItemValidator.NameErrorMessage, orderCommand.ValidationResult.Errors.Select(x => x.ErrorMessage));
        Assert.Contains(AddOrderItemValidator.MinimumQuantityErrorMessage, orderCommand.ValidationResult.Errors.Select(x => x.ErrorMessage));
        Assert.Contains(AddOrderItemValidator.ValueErrorMessage, orderCommand.ValidationResult.Errors.Select(x => x.ErrorMessage));
    }

    [Fact(DisplayName = "Add order item command units greater than permitted")]
    [Trait("Category", "Sales - Order Commands")]
    public void AddOrderItemCommand_UnitsGreaterThanPermitted_ShouldNotPassInTheValidation()
    {
        // Arrange
        var orderCommand = new AddOrderItemCommand(
            Guid.NewGuid(), Guid.NewGuid(), "Test Product", Order.MAX_UNITS_ITEM + 1, 100);

        // Act
        var result = orderCommand.IsValid();

        // Assert
        Assert.False(result);
        Assert.NotEmpty(orderCommand.ValidationResult.Errors);
        Assert.Contains(AddOrderItemValidator.MaximumQuantityErrorMessage, orderCommand.ValidationResult.Errors.Select(x => x.ErrorMessage));
    }
}