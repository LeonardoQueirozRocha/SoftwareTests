using NerdStore.Sales.Domain.Models;

namespace NerdStore.Sales.Domain.Interfaces.Repositories;

public interface IOrderRepository
{
    void Add(Order order);
}