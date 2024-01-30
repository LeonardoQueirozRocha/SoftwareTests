using FluentValidation.Results;
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

        var eventMessage = new AddedOrderItemEvent(
            order.CustomerId,
            order.Id,
            message.ProductId,
            message.Name,
            message.UnitValue,
            message.Quantity);

        order.AddEvent(eventMessage);

        return await _orderRepository.UnitOfWork.Commit();
    }

    public async Task<bool> Handle(UpdateOrderItemCommand message, CancellationToken cancellationToken)
    {
        if (!ValidateCommand(message, cancellationToken)) return false;

        var order = await GetOrderAsync(message.CustomerId, cancellationToken);

        if (order is null) return false;

        var orderItem = await GetOrderItemAsync(order, message.ProductId, cancellationToken);

        if (orderItem is null) return false;

        order.UpdateUnits(orderItem, message.Quantity);

        var eventMessage = new UpdatedOrderProductEvent(
            message.CustomerId,
            order.Id,
            message.ProductId,
            message.Quantity);

        order.AddEvent(eventMessage);

        _orderRepository.UpdateItem(orderItem);
        _orderRepository.Update(order);

        return await _orderRepository.UnitOfWork.Commit();
    }

    public async Task<bool> Handle(RemoveOrderItemCommand message, CancellationToken cancellationToken)
    {
        if (!ValidateCommand(message, cancellationToken)) return false;

        var order = await GetOrderAsync(message.CustomerId, cancellationToken);

        if (order is null) return false;

        var orderItem = await GetOrderItemAsync(order, message.ProductId, cancellationToken);

        if (orderItem is null) return false;

        order.RemoveItem(orderItem);

        var eventMessage = new RemovedOrderProductEvent(
            message.CustomerId,
            order.Id,
            message.ProductId);

        order.AddEvent(eventMessage);

        _orderRepository.RemoveItem(orderItem);
        _orderRepository.Update(order);

        return await _orderRepository.UnitOfWork.Commit();
    }

    public async Task<bool> Handle(ApplyOrderVoucherCommand message, CancellationToken cancellationToken)
    {
        if (!ValidateCommand(message, cancellationToken)) return false;

        var order = await GetOrderAsync(message.CustomerId, cancellationToken);

        if (order is null) return false;

        var voucher = await GetVoucherAsync(message.VoucherCode, cancellationToken);

        if (voucher is null) return false;

        var applyVoucherValidation = order.ApplyVoucher(voucher);

        if (!applyVoucherValidation.IsValid)
        {
            PublishErrorNotifications(applyVoucherValidation.Errors, cancellationToken);
            return false;
        }

        var eventMessage = new AppliedOrderVoucherEvent(message.CustomerId, order.Id, voucher.Id);

        order.AddEvent(eventMessage);

        _orderRepository.Update(order);

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

    private bool ValidateCommand(Command message, CancellationToken cancellationToken)
    {
        if (message.IsValid()) return true;

        PublishErrorNotifications(
            message.ValidationResult.Errors,
            cancellationToken,
            message.MessageType);

        return false;
    }

    private async Task<Order> GetOrderAsync(Guid customerId, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetOrderDraftByCustomerIdAsync(customerId);

        if (order is null)
        {
            await _mediator.Publish(new DomainNotification("order", "Order not found!"), cancellationToken);
            return null;
        }

        return order;
    }

    private async Task<OrderItem> GetOrderItemAsync(
        Order order,
        Guid productId,
        CancellationToken cancellationToken)
    {
        var orderItem = await _orderRepository.GetItemByOrderAsync(order.Id, productId);

        if (!order.OrderItemExists(orderItem))
        {
            await _mediator.Publish(new DomainNotification("Order", "Order item not found!"), cancellationToken);
            return null;
        }

        return orderItem;
    }

    private async Task<Voucher> GetVoucherAsync(string voucherCode, CancellationToken cancellationToken)
    {
        var voucher = await _orderRepository.GetVoucherByCodeAsync(voucherCode);

        if (voucher is null)
        {
            await _mediator.Publish(new DomainNotification("Order", "Voucher not found!"), cancellationToken);
            return null;
        }

        return voucher;
    }

    private void PublishErrorNotifications(
        List<ValidationFailure> errors, 
        CancellationToken cancellationToken, 
        string errorKey = null)
    {

        errors.ForEach(async error =>
        {
            var key = string.IsNullOrEmpty(errorKey) ? error.ErrorCode : errorKey;
            var domainNotification = new DomainNotification(key, error.ErrorMessage);
            await _mediator.Publish(domainNotification, cancellationToken);
        });

    }
}