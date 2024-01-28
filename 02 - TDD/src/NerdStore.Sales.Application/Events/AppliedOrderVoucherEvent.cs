using NerdStore.Core.Messages;

namespace NerdStore.Sales.Application.Events;

public class AppliedOrderVoucherEvent : Event
{
    public Guid CustomerId { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid VoucherId { get; private set; }

    public AppliedOrderVoucherEvent(
        Guid customerId, 
        Guid orderId, 
        Guid voucherId)
    {
        CustomerId = customerId;
        OrderId = orderId;
        VoucherId = voucherId;
    }
}