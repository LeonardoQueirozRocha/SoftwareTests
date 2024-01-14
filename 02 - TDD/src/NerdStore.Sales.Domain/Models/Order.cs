using NerdStore.Core.DomainObjects;
using NerdStore.Sales.Domain.Enums;

namespace NerdStore.Sales.Domain.Models;

public class Order
{
    public static int MAX_UNITS_ITEM => 15;
    public static int MIN_UNITS_ITEM => 1;
    private readonly List<OrderItem> _orderItems;

    public Guid CustomerId { get; private set; }
    public decimal TotalValue { get; private set; }
    public OrderStatus OrderStatus { get; private set; }
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

    protected Order()
    {
        _orderItems = new List<OrderItem>();
    }

    public void MakeDraft()
    {
        OrderStatus = OrderStatus.Draft;
    }

    public void AddItem(OrderItem orderItem)
    {
        ValidateItemPermittedQuantity(orderItem);

        if (OrderItemExists(orderItem))
        {
            var existingItem = GetOrderItemByProductId(orderItem.ProductId);

            ValidateItemPermittedQuantity(orderItem);

            existingItem?.AddUnits(orderItem.Quantity);
            orderItem = existingItem;
            _orderItems.Remove(existingItem);
        }

        _orderItems.Add(orderItem);
        CalculateOrderValue();
    }

    public void UpdateItem(OrderItem orderItem)
    {
        ValidateNonexistentOrderItem(orderItem);
        ValidateItemPermittedQuantity(orderItem);

        var existingItem = GetOrderItemByProductId(orderItem.ProductId);

        _orderItems.Remove(existingItem);
        _orderItems.Add(orderItem);

        CalculateOrderValue();
    }

    public void RemoveItem(OrderItem orderItem)
    {
        ValidateNonexistentOrderItem(orderItem);
        _orderItems.Remove(orderItem);
        CalculateOrderValue();
    }

    public OrderItem GetOrderItemByProductId(Guid productId)
    {
        return _orderItems.FirstOrDefault(o => o.ProductId == productId);
    }

    private void CalculateOrderValue()
    {
        TotalValue = OrderItems.Sum(i => i.CalculateValue());
    }

    private bool OrderItemExists(OrderItem orderItem)
    {
        return _orderItems.Any(o => o.ProductId == orderItem.ProductId);
    }

    private void ValidateNonexistentOrderItem(OrderItem orderItem)
    {
        if (!OrderItemExists(orderItem))
            throw new DomainException("Item don't exists in the order");
    }

    private void ValidateItemPermittedQuantity(OrderItem orderItem)
    {
        var itemQuantity = orderItem.Quantity;
        if (OrderItemExists(orderItem))
        {
            var existingItem = GetOrderItemByProductId(orderItem.ProductId);
            itemQuantity += existingItem.Quantity;
        }

        if (itemQuantity > MAX_UNITS_ITEM)
            throw new DomainException($"Max of {MAX_UNITS_ITEM} units per products");
    }

    public static class OrderFactory
    {
        public static Order NewOrderDraft(Guid customerId)
        {
            var order = new Order { CustomerId = customerId };
            order.MakeDraft();
            return order;
        }
    }
}