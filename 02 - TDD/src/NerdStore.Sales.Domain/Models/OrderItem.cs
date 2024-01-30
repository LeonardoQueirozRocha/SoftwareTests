using NerdStore.Core.DomainObjects;

namespace NerdStore.Sales.Domain.Models;

public class OrderItem : Entity
{
    public Guid ProductId { get; private set; }
    public Guid OrderId { get; private set; }
    public string ProductName { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitValue { get; private set; }

    // EF Rel.
    public Order Order { get; set; }

    public OrderItem(
        Guid productId,
        string productName,
        int quantity,
        decimal unitValue)
    {
        if (quantity < Order.MIN_UNITS_ITEM)
            throw new DomainException($"Min of {Order.MIN_UNITS_ITEM} units per products");

        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitValue = unitValue;
    }

    protected OrderItem() { }

    internal void AssociateOrder(Guid orderId) => OrderId = orderId;

    internal void AddUnits(int units) => Quantity += units;

    internal decimal CalculateValue() => Quantity * UnitValue;

    internal void UpdateUnits(int units) => Quantity = units;
}