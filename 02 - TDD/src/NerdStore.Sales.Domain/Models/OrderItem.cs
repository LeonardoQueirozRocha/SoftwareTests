namespace NerdStore.Sales.Domain.Models;

public class OrderItem
{
    public Guid ProductId { get; }
    public string ProductName { get; }
    public int Quantity { get; }
    public decimal UnitValue { get; }

    public OrderItem(
        Guid productId,
        string productName,
        int quantity,
        decimal unitValue)
    {
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitValue = unitValue;
    }
}