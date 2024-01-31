using NerdStore.WebApp.MVC;
using NerdStore.WebApp.Tests.Configurations;
using NerdStore.WebApp.Tests.Configurations.FixtureCollections;
using Features.Tests.Order;

namespace NerdStore.WebApp.Tests;

[TestCaseOrderer("Features.Tests.Order.PriorityOrderer", "Features.Tests")]
[Collection(nameof(IntegrationWebTestsFixtureCollection))]
public class UserTests
{
    private readonly IntegrationTestsFixture<Program> _testsFixture;

    public UserTests(IntegrationTestsFixture<Program> testsFixture)
    {
        _testsFixture = testsFixture;
    }

    [Fact(DisplayName = "Accomplish register successfully"), TestPriority(1)]
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

    [Fact(DisplayName = "Accomplish login successfully"), TestPriority(2)]
    [Trait("Category", "Web Integration - User")]
    public async Task User_AccomplishLogin_ShouldExecuteSuccessfully()
    {
        // Arrange
        var requestUri = "/Identity/Account/Login";
        var initialResponse = await _testsFixture.Client.GetAsync(requestUri);

        var initialResponseString = await initialResponse.Content.ReadAsStringAsync();
        var antiForgeryToken = _testsFixture.GetAntiForgeryToken(initialResponseString);

        var formData = new Dictionary<string, string>
        {
            { _testsFixture.AntiForgeryFieldName, antiForgeryToken },
            { "Input.Email", _testsFixture.UserEmail },
            { "Input.Password", _testsFixture.UserPassword }
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

    [Fact(DisplayName = "Accomplish register weak password"), TestPriority(3)]
    [Trait("Category", "Web Integration - User")]
    public async Task User_AccomplishRegisterWithWeakPassword_ShouldReturnErrorMessage()
    {
        // Arrange
        var requestUri = "/Identity/Account/Register";

        var initialResponse = await _testsFixture.Client.GetAsync(requestUri);

        initialResponse.EnsureSuccessStatusCode();

        var initialResponseString = await initialResponse.Content.ReadAsStringAsync();
        var antiForgeryToken = _testsFixture.GetAntiForgeryToken(initialResponseString);

        _testsFixture.GenerateUserData();
        const string weakPassword = "1111111111111111111";

        var formData = new Dictionary<string, string>
        {
            { _testsFixture.AntiForgeryFieldName, antiForgeryToken },
            { "Input.Email", _testsFixture.UserEmail },
            { "Input.Password", weakPassword },
            { "Input.ConfirmPassword", weakPassword }
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

        var expectedResults = new List<string>
        {
            "Passwords must have at least one non alphanumeric character.",
            "Passwords must have at least one lowercase (&#x27;a&#x27;-&#x27;z&#x27;).",
            "Passwords must have at least one uppercase (&#x27;A&#x27;-&#x27;Z&#x27;)."
        };

        Assert.All(expectedResults, expectedResult => Assert.Contains(expectedResult, responseString));
    }
}