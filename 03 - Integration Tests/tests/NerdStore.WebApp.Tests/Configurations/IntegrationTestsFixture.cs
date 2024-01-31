using System.Text.RegularExpressions;
using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;

namespace NerdStore.WebApp.Tests.Configurations;

public class IntegrationTestsFixture<TProgram> : IDisposable where TProgram : class
{
    public string AntiForgeryFieldName = "__RequestVerificationToken";
    public string UserEmail;
    public string UserPassword;

    public readonly StoreAppFactory<TProgram> Factory;
    public HttpClient Client;

    public IntegrationTestsFixture()
    {
        var clientOptions = new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = true,
            BaseAddress = new Uri("http://localhost"),
            HandleCookies = true,
            MaxAutomaticRedirections = 7
        };

        Factory = new StoreAppFactory<TProgram>();
        Client = Factory.CreateClient(clientOptions);
    }

    public void GenerateUserData()
    {
        var passwordPrefix = "@1Ab_";
        var locale = "pt_BR";
        var faker = new Faker(locale);

        UserEmail = faker.Internet.Email().ToLower();
        UserPassword = faker.Internet.Password(8, false, string.Empty, passwordPrefix);
    }

    public async Task LoginWebAsync()
    {
        var initialResponse = await Client.GetAsync(WebApplicationUrls.LoginUrl);
        initialResponse.EnsureSuccessStatusCode();

        var htmlBody = await initialResponse.Content.ReadAsStringAsync();
        var antiForgeryToken = GetAntiForgeryToken(htmlBody);

        var formData = new Dictionary<string, string>
        {
            {AntiForgeryFieldName, antiForgeryToken},
            {"Input.Email", "teste@teste.com"},
            {"Input.Password", "Teste@123"}
        };

        var postRequest = new HttpRequestMessage(HttpMethod.Post, WebApplicationUrls.LoginUrl)
        {
            Content = new FormUrlEncodedContent(formData)
        };

        await Client.SendAsync(postRequest);
    }

    public string GetAntiForgeryToken(string htmlBody)
    {
        var pattern = $@"\<input name=""{AntiForgeryFieldName}"" type=""hidden"" value=""([^""]+)"" \/\>";
        var requestVerificationTokenMatch = Regex.Match(htmlBody, pattern);

        if (requestVerificationTokenMatch.Success)
            return requestVerificationTokenMatch.Groups[1].Captures[0].Value;

        var exceptionMessage = $"Anti forgery token '{AntiForgeryFieldName}' not found in HTML";
        throw new ArgumentException(exceptionMessage, nameof(htmlBody));
    }

    public void Dispose()
    {
        Client.Dispose();
        Factory.Dispose();
    }
}