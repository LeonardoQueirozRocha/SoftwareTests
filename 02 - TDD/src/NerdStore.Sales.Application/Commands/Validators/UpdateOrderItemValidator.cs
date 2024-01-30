using FluentValidation;

namespace NerdStore.Sales.Application.Commands.Validators;

public class UpdateOrderItemValidator : AbstractValidator<UpdateOrderItemCommand>
{
    public UpdateOrderItemValidator()
    {
        RuleFor(c => c.CustomerId)
            .NotEqual(Guid.Empty)
            .WithMessage("Customer's Id invalid");

        RuleFor(c => c.ProductId)
            .NotEqual(Guid.Empty)
            .WithMessage("Product Id invalid");

        RuleFor(c => c.Quantity)
            .GreaterThan(0)
            .WithMessage("The minimum quantity of an item is 1");

        RuleFor(c => c.Quantity)
            .LessThan(15)
            .WithMessage("The maximum quantity of an item is 15");
    }
}