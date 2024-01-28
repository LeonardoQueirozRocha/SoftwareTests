using FluentValidation;

namespace NerdStore.Sales.Application.Commands.Validators;

public class ApplyOrderVoucherValidator : AbstractValidator<ApplyOrderVoucherCommand>
{
    public ApplyOrderVoucherValidator()
    {
        RuleFor(c => c.CustomerId)
            .NotEqual(Guid.Empty)
            .WithMessage("Customer's Id invalid");

        RuleFor(c => c.VoucherCode)
            .NotEmpty()
            .WithMessage("Voucher code can't be empty");
    }
}