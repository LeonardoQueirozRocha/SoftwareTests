using System.Linq.Expressions;
using Features.Custumers.Interfaces;
using Features.Custumers.Models;
using Features.Custumers.Services;
using Features.Tests.HumamData;
using MediatR;
using Moq;
using Moq.AutoMock;

namespace Features.Tests.AutoMock;

[Collection(nameof(CustomerBogusCollection))]
public class CustomerServiceAutoMockTests
{
    private readonly CustomerTestsBogusFixture _customerTestsBogusFixture;

    public CustomerServiceAutoMockTests(CustomerTestsBogusFixture customerTestsBogusFixture)
    {
        _customerTestsBogusFixture = customerTestsBogusFixture;
    }

    [Fact(DisplayName = "Add a customer with success")]
    [Trait("Category", "Customer Service AutoMock Tests")]
    public void CustomerService_Add_ShouldExecuteWithSuccess()
    {
        // Arrange
        var customer = _customerTestsBogusFixture.BuildValidCustomer();
        var mocker = new AutoMocker();
        var customerService = mocker.CreateInstance<CustomerService>();

        // Act
        customerService.Add(customer);

        // Assert
        Assert.True(customer.IsValid());

        mocker
            .GetMock<ICustomerRepository>()
            .Verify(
                r => r.Add(customer),
                Times.Once);

        mocker
            .GetMock<IMediator>()
            .Verify(
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
        var mocker = new AutoMocker();
        var customerService = mocker.CreateInstance<CustomerService>();

        // Act
        customerService.Add(customer);

        // Assert
        Assert.False(customer.IsValid());

        mocker
            .GetMock<ICustomerRepository>()
            .Verify(
                r => r.Add(customer),
                Times.Never);

        mocker
            .GetMock<IMediator>()
            .Verify(
                m => m.Publish(It.IsAny<INotification>(),
                CancellationToken.None),
                Times.Never);
    }

    [Fact(DisplayName = "Get Active Customers")]
    [Trait("Category", "Customer Service AutoMock Tests")]
    public void CustomerService_GetAllActive_ShouldReturnOnlyActiveCustomers()
    {
        // Arrange
        var mocker = new AutoMocker();
        var customerService = mocker.CreateInstance<CustomerService>();

        mocker.GetMock<ICustomerRepository>()
            .Setup(c => c.Find(It.IsAny<Expression<Func<Customer, bool>>>()))
            .Returns(_customerTestsBogusFixture.GetVariedCustomers());

        // Act
        var customers = customerService.GetAllActives();

        // Assert
        mocker
            .GetMock<ICustomerRepository>()
            .Verify(
                r => r.Find(It.IsAny<Expression<Func<Customer, bool>>>()), 
                Times.Once);
        
        Assert.True(customers.Any());
        Assert.True(customers.Count(c => !c.Active) > 0);
    }
}