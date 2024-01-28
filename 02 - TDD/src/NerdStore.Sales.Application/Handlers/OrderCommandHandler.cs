using MediatR;
using NerdStore.Core.Messages;
using NerdStore.Core.Messages.CommandMessages.Notifications;
using NerdStore.Sales.Application.Commands;
using NerdStore.Sales.Application.Events;
using NerdStore.Sales.Domain.Interfaces.Repositories;
using NerdStore.Sales.Domain.Models;

namespace NerdStore.Sales.Application.Handlers;

public class OrderCommandHandler :
    IRequestHandler<AddOrderItemCommand, bool>,
    IRequestHandler<UpdateOrderItemCommand, bool>,
    IRequestHandler<RemoveOrderItemCommand, bool>,
    IRequestHandler<ApplyOrderVoucherCommand, bool>
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

    public async Task<bool> Handle(UpdateOrderItemCommand request, CancellationToken cancellationToken)
    {
        // if (!ValidateCommand(request, cancellationToken)) return false;

        // var order = await _orderRepository.GetOrderDraftByCustomerIdAsync(request.CustomerId);

        // if (order is null)
        // {
        //     await _mediator.Publish(new DomainNotification("order", "Order not found!"), cancellationToken);
        //     return false;
        // }
        throw new NotImplementedException();

    }

    public Task<bool> Handle(RemoveOrderItemCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Handle(ApplyOrderVoucherCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
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

    private bool ValidateCommand(Command message, CancellationToken cancellationToken)
    {
        if (message.IsValid()) return true;

        message.ValidationResult.Errors.ForEach(async error =>
            await _mediator.Publish(
                new DomainNotification(message.MessageType, error.ErrorMessage),
                cancellationToken));

        return false;
    }
}