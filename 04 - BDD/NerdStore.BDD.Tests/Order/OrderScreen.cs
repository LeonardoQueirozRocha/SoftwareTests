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

    public int GetStockQuantity()
    {
        var element = Helper.GetElementByXPath("/html/body/div/main/div/div/div[2]/p[1]");
        var quantity = element.Text.OnlyNumbers();

        if (char.IsNumber(quantity.ToString(), 0)) return quantity;

        return 0;
    }

    public void ClickOnBuyNow()
    {
        Helper.ClickByXPath("/html/body/div/main/div/div/div[2]/form/div[2]/button");
    }

    public bool ValidateIsInShppingCart()
    {
        return Helper.ValidateUrlContent(Helper.Configuration.CartUrl);
    }

    public decimal GetProductCartUnitValue()
    {
        return Convert.ToDecimal(Helper
            .GetTextElementById("unitValue")
            .Replace("$", string.Empty)
            .Replace(",", string.Empty)
            .Trim());
    }

    public decimal GetCartTotalValue()
    {
        return Convert.ToDecimal(Helper
            .GetTextElementById("cartTotalValue")
            .Replace("$", string.Empty)
            .Replace(",", string.Empty)
            .Trim());
    }

    public void ClickOnAddItemsQuantity(int quantity)
    {
        var addButton = Helper.GetElementByCssClass("btn-plus");

        if (addButton is null) return;

        for (int i = 1; i < quantity; i++)
        {
            addButton.Click();
        }
    }

    public string GetProductErrorMessage()
    {
        return Helper.GetTextElementByCssClass("alert-danger");
    }

    public void GoToShippingCart()
    {
        Helper.GetElementByXPath("/html/body/header/nav/div/div/ul/li[3]/a").Click();
    }

    public string GetFirstProductInTheShoppingCart()
    {
        return Helper
            .GetElementByXPath("/html/body/div/main/div/div/div/table/tbody/tr[1]/td[1]/div/div/h4/a")
            .GetAttribute("href");
    }

    public void EnsureThatTheFirstItemInTheShowcaseIsAlreadyAdded()
    {
        GoToShippingCart();

        if (GetCartTotalValue() > 0) return;

        AccessProductShowcase();
        GetProductDetail();
        ClickOnBuyNow();
    }

    public int GetFirtCartProductItemsQuantity()
    {
        return Convert.ToInt32(Helper.GetTextBoxValuebyId("quantity"));
    }

    public void BackNavigation(int times = 1)
    {
        Helper.BackNavigation(times);
    }

    public void ResetShoppingCart()
    {
        while (GetCartTotalValue() > 0)
        {
            Helper.ClickByXPath("/html/body/div/main/div/div/div/table/tbody/tr[1]/td[5]/form/button");
        }
    }
}
