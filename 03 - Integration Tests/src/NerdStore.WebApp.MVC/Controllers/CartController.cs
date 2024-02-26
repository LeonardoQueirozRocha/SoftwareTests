using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Catalog.Application.Interfaces.Services;
using NerdStore.Core.Messages.CommandMessages.Notifications;
using NerdStore.Sales.Application.Commands;
using NerdStore.Sales.Application.Interfaces;
using NerdStore.WebApp.MVC.Controllers.Base;

namespace NerdStore.WebApp.MVC.Controllers;

[Authorize]
public class CartController : MainController
{
    private readonly IProductAppService _productAppService;
    private readonly IOrderQueries _orderQueries;
    private readonly IMediator _mediator;

    public CartController(
        INotificationHandler<DomainNotification> notifications,
        IMediator mediator,
        IHttpContextAccessor accessor,
        IProductAppService productAppService,
        IOrderQueries orderQueries) : base(notifications, mediator, accessor)
    {
        _productAppService = productAppService;
        _orderQueries = orderQueries;
        _mediator = mediator;
    }

    [HttpGet("my-cart")]
    public async Task<IActionResult> Index()
    {
        var customerCart = await _orderQueries.GetCustomerCartAsync(CustomerId);
        return View(customerCart);
    }

    [HttpPost("my-cart")]
    public async Task<IActionResult> AddItem(Guid id, int quantity)
    {
        var product = await _productAppService.GetByIdAsync(id);

        if (product is null) BadRequest();

        if (product.QuantityInStock < quantity)
        {
            TempData["Error"] = "Product out of stock";
            return RedirectToAction("ProductDetail", "Showcase", new { id });
        }

        var command = new AddOrderItemCommand(
            CustomerId,
            product.Id,
            product.Name,
            quantity,
            product.Value);

        await _mediator.Send(command);

        if (IsValid()) return RedirectToAction("Index");

        TempData["Errors"] = GetErrorMessages();
        return RedirectToAction("ProductDetail", "Showcase", new { id });
    }

    [HttpPost("remove-item")]
    public async Task<IActionResult> RemoveItem(Guid id)
    {
        var product = await _productAppService.GetByIdAsync(id);

        if (product is null) return BadRequest();

        var command = new RemoveOrderItemCommand(CustomerId, id);

        await _mediator.Send(command);

        if (IsValid()) return RedirectToAction("Index");

        var customerCart = await _orderQueries.GetCustomerCartAsync(CustomerId);

        return View("Index", customerCart);
    }

    [HttpPost("update-item")]
    public async Task<IActionResult> UpdateItem(Guid id, int quantity)
    {
        var product = await _productAppService.GetByIdAsync(id);

        if (product is null) return BadRequest();

        var command = new UpdateOrderItemCommand(CustomerId, id, quantity);

        await _mediator.Send(command);

        if (IsValid()) return RedirectToAction("Index");

        var customerCart = await _orderQueries.GetCustomerCartAsync(CustomerId);

        return View("Index", customerCart);
    }

    [HttpPost("apply-voucher")]
    public async Task<IActionResult> ApplyVoucher(string voucherCode)
    {
        var command = new ApplyOrderVoucherCommand(CustomerId, voucherCode);

        await _mediator.Send(command);

        if (IsValid()) return RedirectToAction("Index");

        var customerCart = await _orderQueries.GetCustomerCartAsync(CustomerId);

        return View("Index", customerCart);
    }

    [HttpGet("order-summary")]
    public async Task<IActionResult> OrderSummary()
    {
        var customerCart = await _orderQueries.GetCustomerCartAsync(CustomerId);
        return View(customerCart);
    }
}