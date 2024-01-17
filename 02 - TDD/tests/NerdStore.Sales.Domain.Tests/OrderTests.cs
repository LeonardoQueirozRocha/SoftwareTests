using NerdStore.Core.DomainObjects;
using NerdStore.Sales.Domain.Enums;
using NerdStore.Sales.Domain.Models;
using Xunit.Sdk;

namespace NerdStore.Sales.Domain.Tests;

public class OrderTests
{
    [Fact(DisplayName = "Add Item New Order")]
    [Trait("Category", "Sales - Order")]
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
    [Trait("Category", "Sales - Order")]
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
        Assert.Equal(3, order.GetOrderItemByProductId(productId).Quantity);
    }

    [Fact(DisplayName = "Add Order Item above permitted")]
    [Trait("Category", "Sales - Order")]
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

    [Fact(DisplayName = "Add existing Order Item above permitted")]
    [Trait("Category", "Sales - Order")]
    public void AddOrderItem_ExistingItemSumUnitsAbovePermitted_ShouldReturnException()
    {
        // Arrange
        var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
        var productId = Guid.NewGuid();
        var orderItem = new OrderItem(productId, "Product Test", 1, 100);
        var orderItem2 = new OrderItem(productId, "Product Test", Order.MAX_UNITS_ITEM, 100);
        order.AddItem(orderItem);

        // Act & Assert
        Assert.Throws<DomainException>(() => order.AddItem(orderItem2));
    }

    [Fact(DisplayName = "Update Nonexistent Order Item")]
    [Trait("Category", "Sales - Order")]
    public void UpdateOrderItem_ItemDoNotExistsInTheList_ShouldReturnAnException()
    {
        // Arrange
        var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
        var updatedOrderItem = new OrderItem(Guid.NewGuid(), "Test Product", 5, 100);

        // Act & Assert
        Assert.Throws<DomainException>(() => order.UpdateItem(updatedOrderItem));
    }

    [Fact(DisplayName = "Update Valid Order Item")]
    [Trait("Category", "Sales - Order")]
    public void UpdateOrderItem_ValidItem_ShouldUpdateQuantity()
    {
        // Arrange
        var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
        var productId = Guid.NewGuid();
        var orderItem = new OrderItem(productId, "Test Product", 2, 100);
        order.AddItem(orderItem);

        var updatedOrderItem = new OrderItem(productId, "Test Product", 5, 100);
        var newQuantity = updatedOrderItem.Quantity;

        // Act
        order.UpdateItem(updatedOrderItem);

        // Assert
        Assert.Equal(newQuantity, order.GetOrderItemByProductId(productId).Quantity);
    }

    [Fact(DisplayName = "Update Order Validate Total Value")]
    [Trait("Category", "Sales - Order")]
    public void UpdateOrderItem_OrderWithDifferentProducts_ShouldUpdateTotalValue()
    {
        // Arrange
        var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
        var productId = Guid.NewGuid();
        var existingOrderItem1 = new OrderItem(Guid.NewGuid(), "Test Product", 2, 100);
        var existingOrderItem2 = new OrderItem(productId, "Test Product", 3, 15);
        order.AddItem(existingOrderItem1);
        order.AddItem(existingOrderItem2);

        var updatedOrderItem = new OrderItem(productId, "Test Product", 5, 15);
        var totalOrder = existingOrderItem1.Quantity * existingOrderItem1.UnitValue +
                         updatedOrderItem.Quantity * updatedOrderItem.UnitValue;

        // Act
        order.UpdateItem(updatedOrderItem);

        // Assert
        Assert.Equal(totalOrder, order.TotalValue);
    }

    [Fact(DisplayName = "Update Order Item Quantity above permitted")]
    [Trait("Category", "Sales - Order")]
    public void UpdateOrderItem_OrderItemUnitsAbovePermitted_ShouldReturnException()
    {
        // Arrange
        var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
        var productId = Guid.NewGuid();
        var existingOrderItem = new OrderItem(productId, "Test Product", 3, 15);
        order.AddItem(existingOrderItem);

        var updatedOrderItem = new OrderItem(productId, "Test Product", Order.MAX_UNITS_ITEM + 1, 15);

        // Act & Assert
        Assert.Throws<DomainException>(() => order.UpdateItem(updatedOrderItem));
    }

    [Fact(DisplayName = "Remove an existing order item")]
    [Trait("Category", "Sales - Order")]
    public void RemoveOrderItem_ItemNotExistedInTheList_ShouldReturnException()
    {
        // Arrange
        var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
        var removeOrderItem = new OrderItem(Guid.NewGuid(), "Test Product", 5, 100);

        // Act & Assert
        Assert.Throws<DomainException>(() => order.RemoveItem(removeOrderItem));
    }

    [Fact(DisplayName = "Remove order item should calculate total value")]
    [Trait("Category", "Sales - Order")]
    public void RemoveOrderItem_ExistedOrderItem_ShouldUpdateTotalValue()
    {
        // Arrange
        var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
        var productId = Guid.NewGuid();
        var orderItem1 = new OrderItem(Guid.NewGuid(), "Xpto Product", 2, 100);
        var orderItem2 = new OrderItem(productId, "Test Product", 3, 15);
        order.AddItem(orderItem1);
        order.AddItem(orderItem2);

        var orderTotalValue = orderItem2.Quantity * orderItem2.UnitValue;

        // Act
        order.RemoveItem(orderItem1);

        // Assert
        Assert.Equal(orderTotalValue, order.TotalValue);
    }

    [Fact(DisplayName = "Apply valid voucher")]
    [Trait("Category", "Sales - Order")]
    public void Order_ApplyValidVoucher_ShouldReturnWithoutErrors()
    {
        // Arrange
        var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
        var voucher = new Voucher(
            code: "PROMO-15-REAIS",
            voucherDiscountType: VoucherDiscountType.Value,
            discountValue: 15,
            discountPercentage: null,
            quantity: 1,
            expirationDate: DateTime.Now.AddDays(15),
            active: true,
            isUsed: false);

        // Act
        var result = order.ApplyVoucher(voucher);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact(DisplayName = "Apply invalid voucher")]
    [Trait("Category", "Sales - Order")]
    public void Order_ApplyInvalidVoucher_ShouldReturnWithErrors()
    {
        // Arrange
        var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
        var voucher = new Voucher(
            code: "PROMO-15-REAIS",
            voucherDiscountType: VoucherDiscountType.Value,
            discountValue: 15,
            discountPercentage: null,
            quantity: 1,
            expirationDate: DateTime.Now.AddDays(-1),
            active: true,
            isUsed: true);

        // Act
        var result = order.ApplyVoucher(voucher);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "Apply voucher discount value")]
    [Trait("Category", "Sales - Order")]
    public void ApplyVoucher_VoucherDiscountValueType_ShouldDiscountTotalValue()
    {
        // Arrange
        var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
        var orderItem1 = new OrderItem(Guid.NewGuid(), "Xpto Product", 2, 100);
        var orderItem2 = new OrderItem(Guid.NewGuid(), "Test Product", 3, 15);

        order.AddItem(orderItem1);
        order.AddItem(orderItem2);

        var voucher = new Voucher(
            code: "PROMO-15-REAIS",
            voucherDiscountType: VoucherDiscountType.Value,
            discountValue: 15,
            discountPercentage: null,
            quantity: 1,
            expirationDate: DateTime.Now.AddDays(10),
            active: true,
            isUsed: false);

        var expectedDiscountValue = order.TotalValue - voucher.DiscountValue;

        // Act
        order.ApplyVoucher(voucher);

        // Assert
        Assert.Equal(expectedDiscountValue, order.TotalValue);
    }

    [Fact(DisplayName = "Apply voucher discount percentage")]
    [Trait("Category", "Sales - Order")]
    public void ApplyVoucher_VoucherDiscountPercentageType_ShouldDiscountTotalValue()
    {
        // Arrange
        var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
        var orderItem1 = new OrderItem(Guid.NewGuid(), "Xpto Product", 2, 100);
        var orderItem2 = new OrderItem(Guid.NewGuid(), "Test Product", 3, 15);

        order.AddItem(orderItem1);
        order.AddItem(orderItem2);

        var voucher = new Voucher(
            code: "PROMO-15-REAIS",
            voucherDiscountType: VoucherDiscountType.Percentage,
            discountValue: null,
            discountPercentage: 15,
            quantity: 1,
            expirationDate: DateTime.Now.AddDays(10),
            active: true,
            isUsed: false);

        var discountValue = order.TotalValue * voucher.DiscountPercentage / 100;
        var expectedDiscountValue = order.TotalValue - discountValue;

        // Act
        order.ApplyVoucher(voucher);

        // Assert
        Assert.Equal(expectedDiscountValue, order.TotalValue);
    }
}