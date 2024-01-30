using MediatR;
using Moq;
using Moq.AutoMock;
using NerdStore.Sales.Application.Commands;
using NerdStore.Sales.Application.Handlers;
using NerdStore.Sales.Domain.Interfaces.Repositories;
using NerdStore.Sales.Domain.Models;

namespace NerdStore.Sales.Application.Tests.Orders;

public class OrderCommandHandlerTests
{
    private readonly Guid _customerId;
    private readonly Guid _productId;
    private readonly Order _order;
    private readonly CancellationToken _cancellationToken;
    private readonly Mock<IOrderRepository> _orderRepository;
    private readonly Mock<IMediator> _mediator;
    private readonly OrderCommandHandler _handler;

    public OrderCommandHandlerTests()
    {
        var mocker = new AutoMocker();
        _handler = mocker.CreateInstance<OrderCommandHandler>();
        _orderRepository = mocker.GetMock<IOrderRepository>();
        _mediator = mocker.GetMock<IMediator>();
        _customerId = Guid.NewGuid();
        _productId = Guid.NewGuid();
        _order = Order.OrderFactory.NewOrderDraft(_customerId);
        _cancellationToken = CancellationToken.None;
    }

    [Fact(DisplayName = "Add item to a order with success")]
    [Trait("Category", "Sales - Order Command Handler")]
    public async Task AddItem_NewOrder_ShouldExecuteWithSuccess()
    {
        // Arrange
        var orderCommand = new AddOrderItemCommand(_customerId, _productId, "Test Product", 2, 100);

        _orderRepository
            .Setup(r => r.UnitOfWork.Commit())
            .Returns(Task.FromResult(true));

        // Act
        var result = await _handler.Handle(orderCommand, _cancellationToken);

        // Assert
        Assert.True(result);

        _orderRepository.Verify(
            r => r.Add(It.IsAny<Order>()),
            Times.Once);

        _orderRepository.Verify(
            r => r.UnitOfWork.Commit(),
            Times.Once);
    }

    [Fact(DisplayName = "Add new order item draft successfully")]
    [Trait("Category", "Sales - Order Command Handler")]
    public async Task AddItem_NewItemToADraftOrder_ShouldExecuteSuccessfully()
    {
        // Arrange
        var existingOrderItem = new OrderItem(Guid.NewGuid(), "Test Product", 2, 100);
        _order.AddItem(existingOrderItem);

        var orderCommand = new AddOrderItemCommand(_customerId, Guid.NewGuid(), "Test Product", 2, 100);

        _orderRepository
            .Setup(r => r.GetOrderDraftByCustomerIdAsync(_customerId))
            .Returns(Task.FromResult(_order));

        _orderRepository
            .Setup(r => r.UnitOfWork.Commit())
            .Returns(Task.FromResult(true));

        // Act
        var result = await _handler.Handle(orderCommand, _cancellationToken);

        // Assert
        Assert.True(result);

        _orderRepository.Verify(
            r => r.AddItem(It.IsAny<OrderItem>()),
            Times.Once);

        _orderRepository.Verify(
            r => r.Update(It.IsAny<Order>()),
            Times.Once);

        _orderRepository.Verify(
            r => r.UnitOfWork.Commit(),
            Times.Once);
    }

    [Fact(DisplayName = "Add existing item to a order draft successfully")]
    [Trait("Category", "Sales - Order Command Handler")]
    public async Task AddItem_ExistingItemToAOrderDraft_ShouldExecuteSuccessfully()
    {
        // Arrange
        var existingOrderItem = new OrderItem(_productId, "Xpto Product", 2, 100);
        _order.AddItem(existingOrderItem);

        var orderCommand = new AddOrderItemCommand(_customerId, _productId, "Xpto Product", 2, 100);

        _orderRepository
            .Setup(r => r.GetOrderDraftByCustomerIdAsync(_customerId))
            .Returns(Task.FromResult(_order));

        _orderRepository
            .Setup(r => r.UnitOfWork.Commit())
            .Returns(Task.FromResult(true));

        // Act
        var result = await _handler.Handle(orderCommand, _cancellationToken);

        // Assert
        Assert.True(result);

        _orderRepository.Verify(
            r => r.UpdateItem(It.IsAny<OrderItem>()),
            Times.Once);

        _orderRepository.Verify(
            r => r.Update(It.IsAny<Order>()),
            Times.Once);

        _orderRepository.Verify(
            r => r.UnitOfWork.Commit(),
            Times.Once);
    }

    [Fact(DisplayName = "Add invalid item command")]
    [Trait("Category", "Sales - Order Command Handler")]
    public async Task AddItem_InvalidCommand_ShouldReturnFalseAndThrowNotificationEvents()
    {
        // Arrange
        var orderCommand = new AddOrderItemCommand(Guid.Empty, Guid.Empty, string.Empty, 0, 0);

        // Act
        var result = await _handler.Handle(orderCommand, _cancellationToken);

        // Assert
        Assert.False(result);

        _mediator.Verify(
            m => m.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()),
            Times.Exactly(5));
    }
}