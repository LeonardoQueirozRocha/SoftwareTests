using FluentValidation;
using NerdStore.Sales.Domain.Enums;
using NerdStore.Sales.Domain.Models;

namespace NerdStore.Sales.Domain.Validators;

public class VoucherApplicableValidator : AbstractValidator<Voucher>
{
    public static string CodeErrorMessage => "Voucher without a valid code";
    public static string ExpirationDateErrorMessage => "This voucher is expired";
    public static string ActiveErrorMessage => "This voucher is not valid anymore";
    public static string IsUsedErrorMessage => "This voucher is already used";
    public static string QuantityErrorMessage => "This voucher is unavailable";
    public static string DiscountValueErrorMessage => "The discount value need to be greater than 0";
    public static string DiscountPercentageErrorMessage => "The discount percentage need to be greater than 0";

    public VoucherApplicableValidator()
    {
        RuleFor(c => c.Code)
            .NotEmpty()
            .WithMessage(CodeErrorMessage);

        RuleFor(c => c.ExpirationDate)
            .Must(ExpirationDateGreaterThanCurrantDate)
            .WithMessage(ExpirationDateErrorMessage);

        RuleFor(c => c.Active)
            .Equal(true)
            .WithMessage(ActiveErrorMessage);

        RuleFor(c => c.IsUsed)
            .Equal(false)
            .WithMessage(IsUsedErrorMessage);

        RuleFor(c => c.Quantity)
            .GreaterThan(0)
            .WithMessage(QuantityErrorMessage);

        When(f => f.VoucherDiscountType is VoucherDiscountType.Value, () =>
        {
            RuleFor(f => f.DiscountValue)
                .NotNull()
                .WithMessage(DiscountValueErrorMessage)
                .GreaterThan(0)
                .WithMessage(DiscountValueErrorMessage);
        });

        When(f => f.VoucherDiscountType is VoucherDiscountType.Percentage, () =>
        {
            RuleFor(f => f.DiscountPercentage)
                .NotNull()
                .WithMessage(DiscountPercentageErrorMessage)
                .GreaterThan(0)
                .WithMessage(DiscountPercentageErrorMessage);
        });
    }

    protected static bool ExpirationDateGreaterThanCurrantDate(DateTime expirationDate) =>
        expirationDate.Date.CompareTo(DateTime.Now.Date) >= 0;
}