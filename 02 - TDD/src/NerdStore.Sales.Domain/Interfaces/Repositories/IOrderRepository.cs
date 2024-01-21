using NerdStore.Core.Data;
using NerdStore.Sales.Domain.Models;

namespace NerdStore.Sales.Domain.Interfaces.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    void Add(Order order);
    void Update(Order order);
    Task<Order> GetOrderDraftByCustomerIdAsync(Guid customerId);
    void AddItem(OrderItem orderItem);
    void UpdateItem(OrderItem orderItem);
}