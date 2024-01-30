using NerdStore.Core.Messages;

namespace NerdStore.Sales.Application.Events;

public class RemovedOrderProductEvent : Event
{
    public Guid CustomerId { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }

    public RemovedOrderProductEvent(
        Guid customerId,
        Guid orderId,
        Guid productId)
    {
        CustomerId = customerId;
        OrderId = orderId;
        ProductId = productId;
    }
}