using NerdStore.Core.DomainObjects;

namespace NerdStore.Sales.Domain.Models;

public class OrderItem
{
    public Guid ProductId { get; }
    public string ProductName { get; }
    public int Quantity { get; private set; }
    public decimal UnitValue { get; }

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

    internal void AddUnits(int units) => Quantity += units;

    internal decimal CalculateValue() => Quantity * UnitValue;
}