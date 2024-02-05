using System.Linq.Expressions;
using Features.Custumers.Interfaces;
using Features.Custumers.Models;
using Features.Custumers.Services;
using FluentAssertions;
using FluentAssertions.Extensions;
using MediatR;
using Moq;

namespace Features.Tests.AutoMock;

[Collection(nameof(CustomerAutoMockerCollection))]
public class CustomerServiceFluentAssertionTests
{
    private readonly CustomerTestsAutoMockerFixture _customerTestsAutoMockerFixture;
    private readonly CustomerService _customerService;
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly Mock<IMediator> _mediatorMock;

    public CustomerServiceFluentAssertionTests(CustomerTestsAutoMockerFixture customerTestsAutoMockerFixture)
    {
        _customerTestsAutoMockerFixture = customerTestsAutoMockerFixture;
        _customerService = _customerTestsAutoMockerFixture.GetCustomerService();
        _customerRepositoryMock = _customerTestsAutoMockerFixture.GetCustomerRepositoryMock();
        _mediatorMock = _customerTestsAutoMockerFixture.GetMediatorMock();
    }

    [Fact(DisplayName = "Add a customer with success")]
    [Trait("Category", "Customer Service Fluent Assertion Tests")]
    public void CustomerService_Add_ShouldExecuteWithSuccess()
    {
        // Arrange
        var customer = _customerTestsAutoMockerFixture.BuildValidCustomer();

        // Act
        _customerService.Add(customer);

        // Assert
        customer.IsValid().Should().BeTrue();        

        _customerRepositoryMock.Verify(
            r => r.Add(customer),
            Times.Once);

        _mediatorMock.Verify(
            m => m.Publish(It.IsAny<INotification>(),
            CancellationToken.None),
            Times.Once);
    }

    [Fact(DisplayName = "Add a customer with failure")]
    [Trait("Category", "Customer Service Fluent Assertion Tests")]
    public void CustomerService_Add_ShouldExecuteWithFailure()
    {
        // Arrange
        var customer = _customerTestsAutoMockerFixture.BuildInvalidCustomer();

        // Act
        _customerService.Add(customer);

        // Assert
        customer.IsValid().Should().BeFalse("Have inconsistencies");
        customer.ValidationResult.Errors.Should().HaveCountGreaterThanOrEqualTo(1);

        _customerRepositoryMock.Verify(
            r => r.Add(customer),
            Times.Never);

        _mediatorMock.Verify(
            m => m.Publish(It.IsAny<INotification>(),
            CancellationToken.None),
            Times.Never);
    }

    [Fact(DisplayName = "Get Active Customers")]
    [Trait("Category", "Customer Service Fluent Assertion Tests")]
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
        
        customers.Should().HaveCountGreaterThanOrEqualTo(1).And.OnlyHaveUniqueItems();
        customers.Should().Contain(c => !c.Active);

        // this assertion is often use in integration tests
        _customerService
            .ExecutionTimeOf(c => c.GetAllActives())
            .Should()
            .BeLessThanOrEqualTo(50.Milliseconds(), "is executed hundred times per seconds");
    }
}