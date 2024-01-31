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

        var initialResponseString = await initialResponse.Content.ReadAsStringAsync();
        var antiForgeryToken = _testsFixture.GetAntiForgeryToken(initialResponseString);

        _testsFixture.GenerateUserData();

        var formData = new Dictionary<string, string>
        {
            { _testsFixture.AntiForgeryFieldName, antiForgeryToken },
            { "Input.Email", _testsFixture.UserEmail },
            { "Input.Password", _testsFixture.UserPassword },
            { "Input.ConfirmPassword", _testsFixture.UserPassword }
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

        var expectedResult = $"Hello {_testsFixture.UserEmail}!";
        
        Assert.Contains(expectedResult, responseString);
    }
}