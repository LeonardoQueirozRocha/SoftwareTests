using Features.Tests.Order;
using NerdStore.WebApp.MVC;
using NerdStore.WebApp.MVC.Models;
using NerdStore.WebApp.Tests.Configurations;
using NerdStore.WebApp.Tests.Configurations.FixtureCollections;
using System.Net.Http.Json;

namespace NerdStore.WebApp.Tests;

[TestCaseOrderer("Features.Tests.Order.PriorityOrderer", "Features.Tests")]
[Collection(nameof(IntegrationApiTestsFixtureCollection))]
public class OrderApiTests
{
    private readonly IntegrationTestsFixture<Program> _testsFixture;

    public OrderApiTests(IntegrationTestsFixture<Program> testsFixture)
    {
        _testsFixture = testsFixture;
    }

    [Fact(DisplayName = "Add new item into a order"), TestPriority(1)]
    [Trait("Category", "API Integration - Order")]
    public async Task AddItem_NewOrder_ShouldReturnSuccessfully()
    {
        // Arrange
        var item = new ItemViewModel
        {
            Id = new Guid("78162be3-61c4-4959-89d7-5ebfb476427e"),
            Quantity = 2
        };

        await _testsFixture.LoginApiAsync();
        _testsFixture.Client.AssignToken(_testsFixture.UserToken);

        // Act
        var postResponse = await _testsFixture.Client.PostAsJsonAsync(ApiApplicationEndpoints.CartEndpoint, item);

        // Assert
        postResponse.EnsureSuccessStatusCode();
    }
}
