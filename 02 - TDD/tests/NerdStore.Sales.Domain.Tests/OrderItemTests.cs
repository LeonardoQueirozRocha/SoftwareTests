using NerdStore.Core.DomainObjects;
using NerdStore.Sales.Domain.Models;

namespace NerdStore.Sales.Domain.Tests;

public class OrderItemTests
{
    [Fact(DisplayName = "New Order Item units below permitted")]
    [Trait("Category", "Order Item Tests")]
    public void AddOrderItem_ItemUnitsBelowPermitted_ShouldReturnException()
    {
        // Arrange Act & Assert
        Assert.Throws<DomainException>(() => new OrderItem(
            Guid.NewGuid(),
            "Order Test",
            Order.MIN_UNITS_ITEM - 1,
            100));
    }
}