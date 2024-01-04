using NerdStore.Sales.Domain.Models;

namespace NerdStore.Sales.Domain.Tests;

public class OrderTests
{
    [Fact(DisplayName = "Add Item New Order")]
    [Trait("Category", "Order Tests")]
    public void AddOrderItem_NewOrder_ShouldUpdateValue()
    {
        // Arrange
        var order = new Order();
        var orderItem = new OrderItem(Guid.NewGuid(), "Test Product", 2, 100);
    
        // Act
        order.AddItem(orderItem);

        // Assert
        Assert.Equal(200, order.TotalValue);
    }
}