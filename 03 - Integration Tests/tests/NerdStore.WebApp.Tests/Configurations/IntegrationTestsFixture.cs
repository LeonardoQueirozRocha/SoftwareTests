using Microsoft.AspNetCore.Mvc.Testing;

namespace NerdStore.WebApp.Tests.Configurations;

public class IntegrationTestsFixture<TProgram> : IDisposable where TProgram : class
{
    public readonly StoreAppFactory<TProgram> Factory;
    public HttpClient Client;

    public IntegrationTestsFixture()
    {
        var clientOptions = new WebApplicationFactoryClientOptions { };

        Factory = new StoreAppFactory<TProgram>();
        Client = Factory.CreateClient(clientOptions);
    }

    public void Dispose()
    {
        Client.Dispose();
        Factory.Dispose();
    }
}