using FluentValidation;

namespace NerdStore.Sales.Application.Commands.Validators;

public class RemoveOrderItemValidator : AbstractValidator<RemoveOrderItemCommand>
{
    public RemoveOrderItemValidator()
    {
        RuleFor(c => c.CustomerId)
            .NotEqual(Guid.Empty)
            .WithMessage("Customer''s Id invalid");

        RuleFor(c => c.ProductId)
            .NotEqual(Guid.Empty)
            .WithMessage("Product Id invalid");
    }
}