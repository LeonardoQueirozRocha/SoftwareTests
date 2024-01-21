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
}