using NerdStore.Core.Data;
using NerdStore.Sales.Domain.Models;

namespace NerdStore.Sales.Domain.Interfaces.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    void Add(Order order);
}