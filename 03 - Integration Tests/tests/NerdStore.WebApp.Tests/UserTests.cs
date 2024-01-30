using NerdStore.WebApp.MVC;
using NerdStore.WebApp.Tests.Configurations;

namespace NerdStore.WebApp.Tests;

public class UserTests
{
    private readonly IntegrationTestsFixture<Program> _testsFixture;

    public UserTests(IntegrationTestsFixture<Program> testsFixture)
    {
        _testsFixture = testsFixture;
    }
}