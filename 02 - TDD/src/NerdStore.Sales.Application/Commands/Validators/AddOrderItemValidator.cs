using FluentValidation;
using NerdStore.Sales.Domain.Models;

namespace NerdStore.Sales.Application.Commands.Validators;

public class AddOrderItemValidator : AbstractValidator<AddOrderItemCommand>
{
    public static string CustomerIdErrorMessage => "Customer Id invalid";
    public static string ProductIdErrorMessage => "Product Id invalid";
    public static string NameErrorMessage => "Product name not informed";
    public static string MaximumQuantityErrorMessage => $"Max quantity of an item is {Order.MAX_UNITS_ITEM}";
    public static string MinimumQuantityErrorMessage => "Minimum quantity of an item is 1";
    public static string ValueErrorMessage => "Item value must be greater than 0";

    public AddOrderItemValidator()
    {
        RuleFor(c => c.CustomerId)
            .NotEqual(Guid.Empty)
            .WithMessage(CustomerIdErrorMessage);

        RuleFor(c => c.ProductId)
            .NotEqual(Guid.Empty)
            .WithMessage(ProductIdErrorMessage);

        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage(NameErrorMessage);

        RuleFor(c => c.Quantity)
            .GreaterThan(0)
            .WithMessage(MinimumQuantityErrorMessage)
            .LessThan(Order.MAX_UNITS_ITEM)
            .WithMessage(MaximumQuantityErrorMessage);

        RuleFor(c => c.UnitValue)
            .GreaterThan(0)
            .WithMessage(ValueErrorMessage);
    }
}