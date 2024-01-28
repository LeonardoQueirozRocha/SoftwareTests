using NerdStore.Core.Messages;
using NerdStore.Sales.Application.Commands.Validators;

namespace NerdStore.Sales.Application.Commands;

public class ApplyOrderVoucherCommand : Command
{
    public Guid CustomerId { get; private set; }
    public string VoucherCode { get; private set; }

    public ApplyOrderVoucherCommand(Guid customerId, string voucherCode)
    {
        CustomerId = customerId;
        VoucherCode = voucherCode;
    }

    public override bool IsValid()
    {
        ValidationResult = new ApplyOrderVoucherValidator().Validate(this);
        return ValidationResult.IsValid;
    }
}