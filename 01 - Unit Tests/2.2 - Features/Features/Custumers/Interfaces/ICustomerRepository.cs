using Features.Core;
using Features.Custumers.Models;

namespace Features.Custumers.Interfaces;

public interface ICustomerRepository : IRepository<Customer>
{
    Customer GetByEmail(string email);
}