using Features.Custumers.Models;

namespace Features.Custumers.Interfaces;

public interface ICustomerService : IDisposable
{
    IEnumerable<Customer> GetAllActives();
    void Add(Customer customer);
    void Update(Customer customer);
    void Remove(Customer customer);
    void Deactivate(Customer customer);
}