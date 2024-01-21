using MediatR;
using NerdStore.Sales.Application.Events;
using NerdStore.Sales.Domain.Interfaces.Repositories;
using NerdStore.Sales.Domain.Models;

namespace NerdStore.Sales.Application.Commands;

public class OrderCommandHandler : IRequestHandler<AddOrderItemCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMediator _mediator;

    public OrderCommandHandler(
        IOrderRepository orderRepository,
        IMediator mediator)
    {
        _orderRepository = orderRepository;
        _mediator = mediator;
    }

    public async Task<bool> Handle(AddOrderItemCommand message, CancellationToken cancellationToken)
    {
        var order = Order.OrderFactory.NewOrderDraft(message.CustomerId);
        var orderItem = new OrderItem(
            message.ProductId,
            message.Name,
            message.Quantity,
            message.UnitValue);

        order.AddItem(orderItem);

        _orderRepository.Add(order);

        var orderEvent = new AddedOrderItemEvent(
            order.CustomerId,
            order.Id,
            message.ProductId,
            message.Name,
            message.UnitValue,
            message.Quantity);

        await _mediator.Publish(orderEvent, cancellationToken);

        return true;
    }
}