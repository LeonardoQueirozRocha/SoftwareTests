using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Sales.Application.Interfaces;

namespace NerdStore.WebApp.MVC.Extensions;

public class CartViewComponent : ViewComponent
{
    private readonly IOrderQueries _orderQueries;

    protected Guid CustomerId;

    public CartViewComponent(
        IOrderQueries orderQueries,
        IHttpContextAccessor accessor)
    {
        _orderQueries = orderQueries;

        if (!accessor.HttpContext.User.Identity.IsAuthenticated) return;

        var claim = accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        CustomerId = Guid.Parse(claim.Value);
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var carrinho = await _orderQueries.GetCustomerCartAsync(CustomerId);
        var itens = carrinho?.Items.Count ?? 0;

        return View(itens);
    }

}