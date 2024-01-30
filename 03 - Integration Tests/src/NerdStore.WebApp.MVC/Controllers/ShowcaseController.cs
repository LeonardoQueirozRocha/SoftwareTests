using MediatR;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Catalog.Application.Interfaces.Services;
using NerdStore.Core.Messages.CommandMessages.Notifications;
using NerdStore.WebApp.MVC.Controllers.Base;

namespace NerdStore.WebApp.MVC.Controllers;

public class ShowcaseController : MainController
{
    private readonly IProductAppService _productAppService;

    public ShowcaseController(
        INotificationHandler<DomainNotification> notifications,
        IMediator mediator, IHttpContextAccessor accessor,
        IProductAppService productAppService) :
            base(notifications, mediator, accessor)
    {
        _productAppService = productAppService;
    }

    [HttpGet]
    [Route("")]
    [Route("showcase")]
    public async Task<IActionResult> Index()
    {
        var products = await _productAppService.GetAllAsync();
        return View(products);
    }

    [HttpGet("product-detail/{id}")]
    public async Task<IActionResult> ProductDetail(Guid id)
    {
        var product = await _productAppService.GetByIdAsync(id);
        return View(product);
    }
}