using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Core.Messages.CommandMessages.Notifications;

namespace NerdStore.WebApp.MVC.Controllers.Base;

public abstract class MainController : Controller
{
    private readonly DomainNotificationHandler _notifications;
    private readonly IMediator _mediator;

    protected Guid CustomerId;

    protected MainController(
        INotificationHandler<DomainNotification> notifications,
        IMediator mediator,
        IHttpContextAccessor accessor)
    {
        _notifications = (DomainNotificationHandler)notifications;
        _mediator = mediator;

        if (!accessor.HttpContext.User.Identity.IsAuthenticated) return;

        var claim = accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        CustomerId = Guid.Parse(claim.Value);
    }

    protected IActionResult CustomResponse(object result = null)
    {
        if (IsValid()) return Ok(new { success = true, data = result });

        return BadRequest(new
        {
            success = false,
            errors = _notifications.GetNotifications().Select(n => n.Value)
        });
    }

    protected bool IsValid()
    {
        return !_notifications.HaveNotification();
    }

    protected IEnumerable<string> GetErrorMessages()
    {
        return _notifications
            .GetNotifications()
            .Select(c => c.Value)
            .ToList();
    }

    protected void NotifyError(string code, string message)
    {
        var notification = new DomainNotification(code, message);
        _mediator.Publish(notification);
    }
}