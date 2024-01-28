using FluentValidation.Results;
using NerdStore.Core.DomainObjects;
using NerdStore.Sales.Domain.Enums;
using NerdStore.Sales.Domain.Validators;

namespace NerdStore.Sales.Domain.Models;

public class Voucher : Entity
{
    public string Code { get; private set; }
    public VoucherDiscountType VoucherDiscountType { get; private set; }
    public decimal? DiscountValue { get; private set; }
    public decimal? DiscountPercentage { get; private set; }
    public int Quantity { get; private set; }
    public DateTime ExpirationDate { get; private set; }
    public bool Active { get; private set; }
    public bool IsUsed { get; private set; }

    // EF Rel.
    public ICollection<Order> Orders { get; set; }

    public Voucher(
        string code,
        VoucherDiscountType voucherDiscountType,
        decimal? discountValue,
        decimal? discountPercentage,
        int quantity,
        DateTime expirationDate,
        bool active,
        bool isUsed)
    {
        Code = code;
        VoucherDiscountType = voucherDiscountType;
        DiscountValue = discountValue;
        DiscountPercentage = discountPercentage;
        Quantity = quantity;
        ExpirationDate = expirationDate;
        Active = active;
        IsUsed = isUsed;
    }

    public ValidationResult ApplyIfApplicable() =>
        new VoucherApplicableValidator().Validate(this);
}
