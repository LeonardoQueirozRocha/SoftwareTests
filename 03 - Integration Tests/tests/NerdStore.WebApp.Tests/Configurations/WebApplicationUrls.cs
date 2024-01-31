namespace NerdStore.WebApp.Tests.Configurations;

public static class WebApplicationUrls
{
    public const string LoginUrl = "/Identity/Account/Login";
    public const string RegisterUrl = "/Identity/Account/Register";
    public const string MyCartUrl = "/my-cart";

    public static string ProductDetailUrl(Guid productId) => $"/product-detail/{productId}";
}