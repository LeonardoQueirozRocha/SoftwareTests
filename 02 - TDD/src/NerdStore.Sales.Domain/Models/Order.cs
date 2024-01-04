namespace NerdStore.Sales.Domain.Models;

public class Order
{
    private readonly List<OrderItem> _orderItems;
    public decimal TotalValue { get; private set; }
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

    public Order()
    {
        _orderItems = new List<OrderItem>();
    }

    public void AddItem(OrderItem orderItem)
    {
        _orderItems.Add(orderItem);
        TotalValue = OrderItems.Sum(i => i.Quantity * i.UnitValue);
    }
}