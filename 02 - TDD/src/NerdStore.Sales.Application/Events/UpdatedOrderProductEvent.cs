using NerdStore.Core.Messages;

namespace NerdStore.Sales.Application.Events;

public class UpdatedOrderProductEvent : Event
{
    public Guid CustomerId { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }

    public UpdatedOrderProductEvent(
        Guid customerId,
        Guid orderId,
        Guid productId,
        int quantity)
    {
        CustomerId = customerId;
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
    }
}