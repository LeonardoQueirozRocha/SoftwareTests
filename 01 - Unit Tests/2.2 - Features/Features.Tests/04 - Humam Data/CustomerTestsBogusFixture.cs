using Bogus;
using Bogus.DataSets;
using Features.Custumers.Models;

namespace Features.Tests.HumamData;

public class CustomerTestsBogusFixture : IDisposable
{
    private const string BrazilianLocale = "pt_BR";

    public Customer BuildValidCustomer()
    {
        return BuildCustomers(1, true).FirstOrDefault();
    }

    public IEnumerable<Customer> GetVariedCustomers()
    {
        var customers = new List<Customer>();

        customers.AddRange(BuildCustomers(50, true));
        customers.AddRange(BuildCustomers(50, false));

        return customers;
    }

    public IEnumerable<Customer> BuildCustomers(int quantity, bool active)
    {
        var gender = new Faker().PickRandom<Name.Gender>();
        var customers = new Faker<Customer>(BrazilianLocale).CustomInstantiator(f => new Customer(
            id: Guid.NewGuid(),
            name: f.Name.FirstName(gender),
            lastName: f.Name.LastName(gender),
            birthDate: f.Date.Past(80, DateTime.Now.AddYears(-18)),
            registrationDate: DateTime.Now,
            email: string.Empty,
            active: active
        )).RuleFor(c => c.Email, (f, c) =>
            f.Internet.Email(
                c.Name.ToLower(), 
                c.LastName.ToLower()));

        return customers.Generate(quantity);
    }

    public Customer BuildInvalidCustomer()
    {
        var gender = new Faker().PickRandom<Name.Gender>();
        var customer = new Faker<Customer>(BrazilianLocale).CustomInstantiator(f => new Customer(
            id: Guid.NewGuid(),
            name: f.Name.FirstName(gender),
            lastName: f.Name.LastName(gender),
            birthDate: f.Date.Past(1, DateTime.Now.AddYears(1)),
            registrationDate: DateTime.Now,
            email: string.Empty,
            active: false
        ));

        return customer;
    }

    public void Dispose() { }
}