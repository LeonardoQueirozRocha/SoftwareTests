using NerdStore.WebApp.MVC;
using NerdStore.WebApp.Tests.Configurations;
using NerdStore.WebApp.Tests.Configurations.FixtureCollections;

namespace NerdStore.WebApp.Tests;

[Collection(nameof(IntegrationWebTestsFixtureCollection))]
public class UserTests
{
    private readonly IntegrationTestsFixture<Program> _testsFixture;

    public UserTests(IntegrationTestsFixture<Program> testsFixture)
    {
        _testsFixture = testsFixture;
    }

    [Fact(DisplayName = "Accomplish register successfully")]
    [Trait("Category", "Web Integration - User")]
    public async Task User_AccomplishRegister_ShouldExecuteSuccessfully()
    {
        // Arrange
        var requestUri = "/Identity/Account/Register";
        var initialResponse = await _testsFixture.Client.GetAsync(requestUri);

        initialResponse.EnsureSuccessStatusCode();

        var email = "teste@teste.com";

        var formData = new Dictionary<string, string>
        {
            { "Input.Email", email },
            { "Input.Password", "Teste@123" },
            { "Input.ConfirmPassword", "Teste@123" }
        };

        var postRequest = new HttpRequestMessage(HttpMethod.Post, requestUri)
        {
            Content = new FormUrlEncodedContent(formData)
        };

        // Act
        var postResponse = await _testsFixture.Client.SendAsync(postRequest);

        // Assert
        var responseString = await postResponse.Content.ReadAsStringAsync();

        postResponse.EnsureSuccessStatusCode();
        Assert.Contains($"Hello {email}!", responseString);
    }
}