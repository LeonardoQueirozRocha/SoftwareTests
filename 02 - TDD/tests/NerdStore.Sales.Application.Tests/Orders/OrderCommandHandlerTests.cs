using MediatR;
using Moq;
using Moq.AutoMock;
using NerdStore.Sales.Application.Commands;
using NerdStore.Sales.Domain.Interfaces.Repositories;
using NerdStore.Sales.Domain.Models;

namespace NerdStore.Sales.Application.Tests.Orders;

public class OrderCommandHandlerTests
{
    [Fact(DisplayName = "Add item to a order with success")]
    [Trait("Category", "Sales - Order Command Handler")]
    public async Task AddItem_NewOrder_ShouldExecuteWithSuccess()
    {
        // Arrange
        var orderCommand = new AddOrderItemCommand(Guid.NewGuid(), Guid.NewGuid(), "Test Product", 2, 100);
        var mocker = new AutoMocker();
        var orderHandler = mocker.CreateInstance<OrderCommandHandler>();

        mocker
            .GetMock<IOrderRepository>()
            .Setup(r => r.UnitOfWork.Commit())
            .Returns(Task.FromResult(true));

        // Act
        var result = await orderHandler.Handle(orderCommand, CancellationToken.None);

        // Assert
        Assert.True(result);

        mocker.GetMock<IOrderRepository>().Verify(
            r => r.Add(It.IsAny<Order>()),
            Times.Once);

        mocker.GetMock<IOrderRepository>().Verify(
            r => r.UnitOfWork.Commit(),
            Times.Once);
    }

    [Fact(DisplayName = "Add new order item draft successfully")]
    [Trait("Category", "Sales - Order Command Handler")]
    public async Task AddItem_NewItemToADraftOrder_ShouldExecuteSuccessfully()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var order = Order.OrderFactory.NewOrderDraft(customerId);
        var existingOrderItem = new OrderItem(Guid.NewGuid(), "Test Product", 2, 100);
        order.AddItem(existingOrderItem);

        var orderCommand = new AddOrderItemCommand(customerId, Guid.NewGuid(), "Test Product", 2, 100);

        var mocker = new AutoMocker();
        var orderHandler = mocker.CreateInstance<OrderCommandHandler>();

        mocker
            .GetMock<IOrderRepository>()
            .Setup(r => r.GetOrderDraftByCustomerIdAsync(customerId))
            .Returns(Task.FromResult(order));

        mocker
            .GetMock<IOrderRepository>()
            .Setup(r => r.UnitOfWork.Commit())
            .Returns(Task.FromResult(true));

        // Act
        var result = await orderHandler.Handle(orderCommand, CancellationToken.None);

        // Assert
        Assert.True(result);

        mocker.GetMock<IOrderRepository>().Verify(
            r => r.AddItem(It.IsAny<OrderItem>()),
            Times.Once);

        mocker.GetMock<IOrderRepository>().Verify(
            r => r.Update(It.IsAny<Order>()),
            Times.Once);

        mocker.GetMock<IOrderRepository>().Verify(
            r => r.UnitOfWork.Commit(),
            Times.Once);
    }

    [Fact(DisplayName = "Add existing item to a order draft successfully")]
    [Trait("Category", "Sales - Order Command Handler")]
    public async Task AddItem_ExistingItemToAOrderDraft_ShouldExecuteSuccessfully()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var order = Order.OrderFactory.NewOrderDraft(customerId);
        var existingOrderItem = new OrderItem(productId, "Xpto Product", 2, 100);
        order.AddItem(existingOrderItem);

        var orderCommand = new AddOrderItemCommand(customerId, productId, "Xpto Product", 2, 100);

        var mocker = new AutoMocker();
        var orderHandler = mocker.CreateInstance<OrderCommandHandler>();

        mocker
            .GetMock<IOrderRepository>()
            .Setup(r => r.GetOrderDraftByCustomerIdAsync(customerId))
            .Returns(Task.FromResult(order));

        mocker
            .GetMock<IOrderRepository>()
            .Setup(r => r.UnitOfWork.Commit())
            .Returns(Task.FromResult(true));

        // Act
        var result = await orderHandler.Handle(orderCommand, CancellationToken.None);

        // Assert
        Assert.True(result);

        mocker.GetMock<IOrderRepository>().Verify(
            r => r.UpdateItem(It.IsAny<OrderItem>()),
            Times.Once);

        mocker.GetMock<IOrderRepository>().Verify(
            r => r.Update(It.IsAny<Order>()),
            Times.Once);

        mocker.GetMock<IOrderRepository>().Verify(
            r => r.UnitOfWork.Commit(),
            Times.Once);
    }

    [Fact(DisplayName = "Add invalid item command")]
    [Trait("Category", "Sales - Order Command Handler")]
    public async Task AddItem_InvalidCommand_ShouldReturnFalseAndThrowNotificationEvents()
    {
        // Arrange
        var orderCommand = new AddOrderItemCommand(Guid.Empty, Guid.Empty, string.Empty, 0, 0);

        var mocker = new AutoMocker();
        var orderHandler = mocker.CreateInstance<OrderCommandHandler>();

        // Act
        var result = await orderHandler.Handle(orderCommand, CancellationToken.None);

        // Assert
        Assert.False(result);
        mocker.GetMock<IMediator>().Verify(
            m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), 
            Times.Exactly(5));
    }
}