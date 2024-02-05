using Features.Custumers.Interfaces;
using Features.Custumers.Models;
using Features.Custumers.Notifications;
using MediatR;

namespace Features.Custumers.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMediator _mediator;

    public CustomerService(
        ICustomerRepository customerRepository,
        IMediator mediator)
    {
        _customerRepository = customerRepository;
        _mediator = mediator;
    }

    public IEnumerable<Customer> GetAllActives()
    {
        return _customerRepository.Find(c => c.Active);
    }

    public void Add(Customer customer)
    {
        if (!customer.IsValid()) return;

        _customerRepository.Add(customer);
        _mediator.Publish(new CustomerEmailNotification("admin@me.com", customer.Email, "Olá", "Bem vindo!"));
    }

    public void Update(Customer customer)
    {
        if (!customer.IsValid()) return;

        _customerRepository.Update(customer);
        _mediator.Publish(new CustomerEmailNotification("admin@me.com", customer.Email, "Mudanças", "Dê uma olhada!"));
    }

    public void Remove(Customer customer)
    {
        _customerRepository.Remove(customer.Id);
        _mediator.Publish(new CustomerEmailNotification("admin@me.com", customer.Email, "Adeus", "Tenha uma boa jornada!"));
    }

    public void Deactivate(Customer customer)
    {
        if (!customer.IsValid()) return;

        customer.Deactivate();
        _customerRepository.Update(customer);
        _mediator.Publish(new CustomerEmailNotification("admin@me.com", customer.Email, "Até breve", "Até mais tarde!"));
    }

    public void Dispose()
    {
        _customerRepository.Dispose();
    }
}