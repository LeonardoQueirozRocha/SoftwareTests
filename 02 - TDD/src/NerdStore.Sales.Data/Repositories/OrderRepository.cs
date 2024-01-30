using Microsoft.EntityFrameworkCore;
using NerdStore.Core.Data;
using NerdStore.Sales.Domain.Enums;
using NerdStore.Sales.Domain.Interfaces.Repositories;
using NerdStore.Sales.Domain.Models;

namespace NerdStore.Sales.Data.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly SalesContext _context;

    public OrderRepository(SalesContext context)
    {
        _context = context;
    }

    public IUnitOfWork UnitOfWork => _context;

    public async Task<IEnumerable<Order>> GetListByCustomerIdAsync(Guid customerId)
    {
        return await _context.Orders
            .AsNoTracking()
            .Where(p => p.CustomerId == customerId)
            .ToListAsync();
    }

    public async Task<Order> GetOrderDraftByCustomerIdAsync(Guid customerId)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(p => p.CustomerId == customerId && p.OrderStatus == OrderStatus.Draft);

        if (order is null) return null;

        await _context
            .Entry(order)
            .Collection(i => i.OrderItems)
            .LoadAsync();

        if (order.VoucherId is not null)
            await _context
                .Entry(order)
                .Reference(i => i.Voucher)
                .LoadAsync();

        return order;
    }

    public void Add(Order order)
    {
        _context.Orders.Add(order);
    }

    public void Update(Order order)
    {
        _context.Orders.Update(order);
    }

    public async Task<OrderItem> GetItemByOrderAsync(Guid orderId, Guid productId)
    {
        return await _context.OrderItems.FirstOrDefaultAsync(p => p.ProductId == productId && p.OrderId == orderId);
    }

    public void AddItem(OrderItem orderItem)
    {
        _context.OrderItems.Add(orderItem);
    }

    public void UpdateItem(OrderItem orderItem)
    {
        _context.OrderItems.Update(orderItem);
    }

    public void RemoveItem(OrderItem orderItem)
    {
        _context.OrderItems.Remove(orderItem);
    }

    public async Task<Voucher> GetVoucherByCodeAsync(string code)
    {
        return await _context.Vouchers.FirstOrDefaultAsync(p => p.Code == code);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}