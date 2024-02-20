using NerdStore.BDD.Tests.Configurations;

namespace NerdStore.BDD.Tests.Order;

public class OrderScreen : PageObjectModel
{
    public OrderScreen(SeleniumHelper helper) : base(helper) { }

    public void AccessProductShowcase()
    {
        Helper.GoToUrl(Helper.Configuration.ShowcaseUrl);
    }

    public void GetProductDetail(int position = 1)
    {
        Helper.ClickByXPath($"/html/body/div/main/div/div/div[{position}]/span/a");
    }

    public bool ValidateAvailableProduct()
    {
        return Helper.ValidateUrlContent(Helper.Configuration.ProductUrl);
    }
}
