using AngleSharp.Html.Parser;
using NerdStore.WebApp.MVC;
using NerdStore.WebApp.Tests.Configurations;
using NerdStore.WebApp.Tests.Configurations.FixtureCollections;

namespace NerdStore.WebApp.Tests;

[Collection(nameof(IntegrationWebTestsFixtureCollection))]
public class OrderWebTests
{
    private readonly IntegrationTestsFixture<Program> _testsFixture;

    public OrderWebTests(IntegrationTestsFixture<Program> testsFixture)
    {
        _testsFixture = testsFixture;
    }

    [Fact(DisplayName = "Add item in a new order")]
    [Trait("Category", "Web Integration - Order")]
    public async Task AddItem_NewOrder_ShouldUpdateTotalValue()
    {
        // Arrange
        var productId = new Guid("191ddd3e-acd4-4c3b-ae74-8e473993c5da");
        const int quantity = 2;

        var initialResponse = await _testsFixture.Client.GetAsync(WebApplicationUrls.ProductDetailUrl(productId));
        initialResponse.EnsureSuccessStatusCode();

        var formData = new Dictionary<string, string>
        {
            {"Id", productId.ToString()},
            {"quantity", quantity.ToString()}
        };

        await _testsFixture.LoginWebAsync();

        var postRequest = new HttpRequestMessage(HttpMethod.Post, WebApplicationUrls.MyCartUrl)
        {
            Content = new FormUrlEncodedContent(formData)
        };

        // Act
        var postResponse = await _testsFixture.Client.SendAsync(postRequest);

        // Assert
        var postResponseString = await postResponse.Content.ReadAsStringAsync();
        var html = new HtmlParser()
            .ParseDocumentAsync(postResponseString)
            .Result
            .All;

        var formQuantity = html?
            .FirstOrDefault(c => c.Id == "quantity")?
            .GetAttribute("value");
    }
}