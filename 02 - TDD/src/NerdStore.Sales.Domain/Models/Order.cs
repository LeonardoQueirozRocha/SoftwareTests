using FluentValidation.Results;
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
    public decimal Discount { get; private set; }
    public OrderStatus OrderStatus { get; private set; }
    public bool IsVoucherUsed { get; private set; }
    public Voucher Voucher { get; private set; }
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

    public ValidationResult ApplyVoucher(Voucher voucher)
    {
        var result = voucher.ApplyIfApplicable();

        if (!result.IsValid) return result;

        Voucher = voucher;
        IsVoucherUsed = true;

        CalculateDiscountTotalValue();

        return result;
    }

    public void CalculateDiscountTotalValue()
    {
        if (!IsVoucherUsed) return;

        decimal discount = 0;
        var value = TotalValue;

        if (Voucher.VoucherDiscountType is VoucherDiscountType.Value)
        {
            discount = Voucher.DiscountValue ?? 0;
            value -= discount;
        }
        else
        {
            discount = TotalValue * (Voucher.DiscountPercentage ?? 0) / 100;
            value -= discount;
        }

        TotalValue = value < 0 ? 0 : value;
        Discount = discount;
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

    private void CalculateOrderValue()
    {
        TotalValue = OrderItems.Sum(i => i.CalculateValue());
        CalculateDiscountTotalValue();
    }

    private bool OrderItemExists(OrderItem orderItem)
    {
        return _orderItems.Any(o => o.ProductId == orderItem.ProductId);
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