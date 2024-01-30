using NerdStore.Sales.Application.ViewModels;

namespace NerdStore.Sales.Application.Interfaces;

public interface IOrderQueries
{
    Task<CartViewModel> GetCustomerCartAsync(Guid customerId);
    Task<IEnumerable<OrderViewModel>> GetCustomerOrdersAsync(Guid customerId);
}