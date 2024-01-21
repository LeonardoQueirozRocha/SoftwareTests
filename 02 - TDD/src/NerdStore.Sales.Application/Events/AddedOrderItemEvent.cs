using NerdStore.Core.Messages;

namespace NerdStore.Sales.Application.Events;

public class AddedOrderItemEvent : Event
{
    public Guid CustomerId { get; }
    public Guid OrderId { get; }
    public Guid ProductId { get; }
    public string ProductName { get; }
    public decimal UnitValue { get; }
    public int Quantity { get; }

    public AddedOrderItemEvent(
        Guid customerId,
        Guid orderId,
        Guid productId,
        string productName,
        decimal unitValue,
        int quantity)
    {
        CustomerId = customerId;
        OrderId = orderId;
        ProductId = productId;
        ProductName = productName;
        UnitValue = unitValue;
        Quantity = quantity;
    }
}