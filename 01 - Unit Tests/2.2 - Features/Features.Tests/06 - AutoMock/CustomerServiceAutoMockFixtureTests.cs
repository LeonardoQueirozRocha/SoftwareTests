using System.Linq.Expressions;
using Features.Custumers.Interfaces;
using Features.Custumers.Models;
using Features.Custumers.Services;
using MediatR;
using Moq;

namespace Features.Tests.AutoMock;

[Collection(nameof(CustomerAutoMockerCollection))]
public class CustomerServiceAutoMockFixtureTests
{
    private readonly CustomerTestsAutoMockerFixture _customerTestsAutoMockerFixture;
    private readonly CustomerService _customerService;
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly Mock<IMediator> _mediatorMock;

    public CustomerServiceAutoMockFixtureTests(CustomerTestsAutoMockerFixture customerTestsAutoMockerFixture)
    {
        _customerTestsAutoMockerFixture = customerTestsAutoMockerFixture;
        _customerService = _customerTestsAutoMockerFixture.GetCustomerService();
        _customerRepositoryMock = _customerTestsAutoMockerFixture.GetCustomerRepositoryMock();
        _mediatorMock = _customerTestsAutoMockerFixture.GetMediatorMock();
    }

    [Fact(DisplayName = "Add a customer with success")]
    [Trait("Category", "Customer Service AutoMockFixture Tests")]
    public void CustomerService_Add_ShouldExecuteWithSuccess()
    {
        // Arrange
        var customer = _customerTestsAutoMockerFixture.BuildValidCustomer();

        // Act
        _customerService.Add(customer);

        // Assert
        Assert.True(customer.IsValid());

        _customerRepositoryMock.Verify(
            r => r.Add(customer),
            Times.Once);

        _mediatorMock.Verify(
            m => m.Publish(It.IsAny<INotification>(),
            CancellationToken.None),
            Times.Once);
    }

    [Fact(DisplayName = "Add a customer with failure")]
    [Trait("Category", "Customer Service AutoMockFixture Tests")]
    public void CustomerService_Add_ShouldExecuteWithFailure()
    {
        // Arrange
        var customer = _customerTestsAutoMockerFixture.BuildInvalidCustomer();

        // Act
        _customerService.Add(customer);

        // Assert
        Assert.False(customer.IsValid());

        _customerRepositoryMock.Verify(
            r => r.Add(customer),
            Times.Never);

        _mediatorMock.Verify(
            m => m.Publish(It.IsAny<INotification>(),
            CancellationToken.None),
            Times.Never);
    }

    [Fact(DisplayName = "Get Active Customers")]
    [Trait("Category", "Customer Service AutoMockFixture Tests")]
    public void CustomerService_GetAllActive_ShouldReturnOnlyActiveCustomers()
    {
        // Arrange
        _customerRepositoryMock
            .Setup(c => c.Find(It.IsAny<Expression<Func<Customer, bool>>>()))
            .Returns(_customerTestsAutoMockerFixture.GetVariedCustomers());

        // Act
        var customers = _customerService.GetAllActives();

        // Assert
        _customerRepositoryMock.Verify(
            r => r.Find(It.IsAny<Expression<Func<Customer, bool>>>()), 
            Times.Once);
        
        Assert.True(customers.Any());
        Assert.True(customers.Count(c => !c.Active) > 0);
    }
}