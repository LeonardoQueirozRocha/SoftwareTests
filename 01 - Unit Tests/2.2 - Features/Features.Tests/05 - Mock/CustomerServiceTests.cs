using System.Linq.Expressions;
using Features.Costumers.Interfaces;
using Features.Costumers.Models;
using Features.Costumers.Services;
using Features.Tests.HumamData;
using MediatR;
using Moq;

namespace Features.Tests.Mock;

[Collection(nameof(CustomerBogusCollection))]
public class CustomerServiceTests
{
    private readonly CustomerTestsBogusFixture _customerTestsBogusFixture;

    public CustomerServiceTests(CustomerTestsBogusFixture customerTestsBogusFixture)
    {
        _customerTestsBogusFixture = customerTestsBogusFixture;
    }

    [Fact(DisplayName = "Add a customer with success")]
    [Trait("Category", "Customer Service Mock Tests")]
    public void CustomerService_Add_ShouldExecuteWithSuccess()
    {
        // Arrange
        var customer = _customerTestsBogusFixture.BuildValidCustomer();
        var customerRepo = new Mock<ICustomerRepository>();
        var mediator = new Mock<IMediator>();
        var customerService = new CustomerService(
            customerRepo.Object,
            mediator.Object);

        // Act
        customerService.Add(customer);

        // Assert
        Assert.True(customer.IsValid());

        customerRepo.Verify(
            r => r.Add(customer),
            Times.Once);

        mediator.Verify(
            m => m.Publish(It.IsAny<INotification>(),
            CancellationToken.None),
            Times.Once);
    }

    [Fact(DisplayName = "Add a customer with failure")]
    [Trait("Category", "Customer Service Mock Tests")]
    public void CustomerService_Add_ShouldExecuteWithFailure()
    {
        // Arrange
        var customer = _customerTestsBogusFixture.BuildInvalidCustomer();
        var customerRepo = new Mock<ICustomerRepository>();
        var mediator = new Mock<IMediator>();
        var customerService = new CustomerService(
            customerRepo.Object,
            mediator.Object);

        // Act
        customerService.Add(customer);

        // Assert
        Assert.False(customer.IsValid());

        customerRepo.Verify(
            r => r.Add(customer),
            Times.Never);

        mediator.Verify(
            m => m.Publish(It.IsAny<INotification>(),
            CancellationToken.None),
            Times.Never);
    }

    [Fact(DisplayName = "Get Active Customers")]
    [Trait("Category", "Customer Service Mock Tests")]
    public void CustomerService_GetAllActive_ShouldReturnOnlyActiveCustomers()
    {
        // Arrange
        var customerRepo = new Mock<ICustomerRepository>();
        var mediator = new Mock<IMediator>();
        var customerService = new CustomerService(
            customerRepo.Object,
            mediator.Object);

        customerRepo
            .Setup(c => c.Find(It.IsAny<Expression<Func<Customer, bool>>>()))
            .Returns(_customerTestsBogusFixture.GetVariedCustomers());

        // Act
        var customers = customerService.GetAllActives();

        // Assert
        customerRepo.Verify(
            r => r.Find(It.IsAny<Expression<Func<Customer, bool>>>()), 
            Times.Once);
        
        Assert.True(customers.Any());
        Assert.True(customers.Count(c => !c.Active) > 0);
    }
}