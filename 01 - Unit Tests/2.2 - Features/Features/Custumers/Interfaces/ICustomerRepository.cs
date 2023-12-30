using Features.Core;
using Features.Costumers.Models;

namespace Features.Costumers.Interfaces;

public interface ICustomerRepository : IRepository<Customer>
{
    Customer GetByEmail(string email);
}