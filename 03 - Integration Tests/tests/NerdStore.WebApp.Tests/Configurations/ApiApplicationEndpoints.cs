namespace NerdStore.WebApp.Tests.Configurations;

public static class ApiApplicationEndpoints
{
    public const string CartEndpoint = "api/cart";
    public const string LoginEndpoint = "api/login";
    public static string DeleteEndpoint(Guid productId) => $"api/cart/{productId}";
}