using Features.Costumers.Models;

namespace Features.Tests.Fixtures;

public class CustomerTestsFixture : IDisposable
{
    public Customer BuildValidCustomer()
    {
        var customer = new Customer(
            id: Guid.NewGuid(),
            name: "Leonardo",
            lastName: "Rocha",
            birthDate: DateTime.Now.AddYears(-25),
            registrationDate: DateTime.Now,
            email: "leo@leo.com",
            active: true);

        return customer;
    }
    
    public Customer BuildInvalidCustomer()
    {
        var customer = new Customer(
            id: Guid.NewGuid(),
            name: string.Empty,
            lastName: string.Empty,
            birthDate: DateTime.Now,
            registrationDate: DateTime.Now,
            email: "leo2@leo.com",
            active: true);

        return customer;
    }

    public void Dispose() { }
}