using NerdStore.Core.Messages;
using NerdStore.Sales.Application.Commands.Validators;

namespace NerdStore.Sales.Application.Commands;

public class AddOrderItemCommand : Command
{
    public Guid CustomerId { get; private set; }
    public Guid ProductId { get; private set; }
    public string Name { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitValue { get; private set; }

    public AddOrderItemCommand(
        Guid customerId,
        Guid productId,
        string name,
        int quantity,
        decimal unitValue)
    {
        CustomerId = customerId;
        ProductId = productId;
        Name = name;
        Quantity = quantity;
        UnitValue = unitValue;
    }

    public override bool IsValid()
    {
        ValidationResult = new AddOrderItemValidator().Validate(this);
        return ValidationResult.IsValid;
    }
}