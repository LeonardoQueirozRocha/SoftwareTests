using NerdStore.Core.DomainObjects;
using NerdStore.Sales.Domain.Models;

namespace NerdStore.Sales.Domain.Tests;

public class OrderTests
{
    [Fact(DisplayName = "Add Item New Order")]
    [Trait("Category", "Order Tests")]
    public void AddOrderItem_NewOrder_ShouldUpdateValue()
    {
        // Arrange
        var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
        var orderItem = new OrderItem(Guid.NewGuid(), "Test Product", 2, 100);

        // Act
        order.AddItem(orderItem);

        // Assert
        Assert.Equal(200, order.TotalValue);
    }

    [Fact(DisplayName = "Add Existing Order Item")]
    [Trait("Category", "Order Tests")]
    public void AddOrderItem_ExistingItem_ShouldIncrementUnitsSumValues()
    {
        // Arrange
        var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
        var productId = Guid.NewGuid();
        var orderItem = new OrderItem(productId, "Test Product", 2, 100);
        var orderItem2 = new OrderItem(productId, "Test Product", 1, 100);
        order.AddItem(orderItem);

        // Act
        order.AddItem(orderItem2);

        // Assert
        Assert.Equal(300, order.TotalValue);
        Assert.Equal(1, order.OrderItems.Count);
        Assert.Equal(3, order.OrderItems?.FirstOrDefault(p => p.ProductId == productId)?.Quantity);
    }

    [Fact(DisplayName = "Add Order Item above permitted")]
    [Trait("Category", "Order Tests")]
    public void AddOrderItem_ItemUnitsAbovePermitted_ShouldReturnException()
    {
        // Arrange
        var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
        var productId = Guid.NewGuid();
        var orderItem = new OrderItem(
            productId,
            "Order Test",
            Order.MAX_UNITS_ITEM + 1,
            100);

        // Act & Assert
        Assert.Throws<DomainException>(() => order.AddItem(orderItem));
    }
}