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

    public void MakeDraft() => OrderStatus = OrderStatus.Draft;

    public void AddItem(OrderItem orderItem)
    {
        if (orderItem.Quantity > MAX_UNITS_ITEM) 
            throw new DomainException($"Max of {MAX_UNITS_ITEM} units per products");

        if (_orderItems.Any(o => o.ProductId == orderItem.ProductId))
        {
            var existingItem = _orderItems.FirstOrDefault(o => o.ProductId == orderItem.ProductId);
            existingItem?.AddUnits(orderItem.Quantity);
            orderItem = existingItem;
            _orderItems.Remove(existingItem);
        }

        _orderItems.Add(orderItem);
        CalculateOrderValue();
    }

    private void CalculateOrderValue() => TotalValue = OrderItems.Sum(i => i.CalculateValue());

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