using MediatR;
using NerdStore.Core.Messages.CommandMessages.Notifications;
using NerdStore.Sales.Application.Events;
using NerdStore.Sales.Domain.Interfaces.Repositories;
using NerdStore.Sales.Domain.Models;

namespace NerdStore.Sales.Application.Commands;

public class OrderCommandHandler : IRequestHandler<AddOrderItemCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMediator _mediator;

    public OrderCommandHandler(IOrderRepository orderRepository, IMediator mediator)
    {
        _orderRepository = orderRepository;
        _mediator = mediator;
    }

    public async Task<bool> Handle(AddOrderItemCommand message, CancellationToken cancellationToken)
    {
        if (!ValidateCommand(message, cancellationToken)) return false;

        var order = await _orderRepository.GetOrderDraftByCustomerIdAsync(message.CustomerId);
        var orderItem = new OrderItem(
            message.ProductId,
            message.Name,
            message.Quantity,
            message.UnitValue);

        if (order == null)
        {
            order = AddNewOrderDraft(message.CustomerId, orderItem);
        }
        else
        {
            UpdateExistingOrder(order, orderItem);
        }

        var eventItem = new AddedOrderItemEvent(
            order.CustomerId,
            order.Id,
            message.ProductId,
            message.Name,
            message.UnitValue,
            message.Quantity);

        order.AddEvent(eventItem);

        return await _orderRepository.UnitOfWork.Commit();
    }

    private Order AddNewOrderDraft(Guid customerId, OrderItem orderItem)
    {
        var order = Order.OrderFactory.NewOrderDraft(customerId);
        order.AddItem(orderItem);
        _orderRepository.Add(order);

        return order;
    }

    private void UpdateExistingOrder(Order order, OrderItem orderItem)
    {
        var orderItemExists = order.OrderItemExists(orderItem);
        order.AddItem(orderItem);

        if (orderItemExists)
        {
            _orderRepository.UpdateItem(order.GetOrderItemByProductId(orderItem.ProductId));
        }
        else
        {
            _orderRepository.AddItem(orderItem);
        }

        _orderRepository.Update(order);
    }

    private bool ValidateCommand(AddOrderItemCommand message, CancellationToken cancellationToken)
    {
        if (message.IsValid()) return true;

        message.ValidationResult.Errors.ForEach(async error =>
            await _mediator.Publish(
                new DomainNotification(message.MessageType, error.ErrorMessage),
                cancellationToken));

        return false;
    }
}