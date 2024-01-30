using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NerdStore.Catalog.Application.Interfaces.Services;
using NerdStore.Core.Messages.CommandMessages.Notifications;
using NerdStore.Sales.Application.Commands;
using NerdStore.Sales.Application.Interfaces;
using NerdStore.WebApp.MVC.Configurations;
using NerdStore.WebApp.MVC.Controllers.Base;
using NerdStore.WebApp.MVC.Models;

namespace NerdStore.WebApp.MVC.Controllers.API;

[ApiController]
[Authorize]
public class CartControllerAPI : MainController
{
    private readonly IProductAppService _productAppService;
    private readonly IOrderQueries _orderQueries;
    private readonly IMediator _mediator;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly AppSettings _appSettings;

    public CartControllerAPI(
        INotificationHandler<DomainNotification> notifications,
        IMediator mediator,
        IHttpContextAccessor accessor,
        IProductAppService productAppService,
        IOrderQueries orderQueries,
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager,
        IOptions<AppSettings> appSettings) : base(notifications, mediator, accessor)
    {
        _productAppService = productAppService;
        _orderQueries = orderQueries;
        _signInManager = signInManager;
        _userManager = userManager;
        _appSettings = appSettings.Value;
    }

    [HttpGet("api/cart")]
    public async Task<IActionResult> GetCart()
    {
        return CustomResponse(await _orderQueries.GetCustomerCartAsync(CustomerId));
    }

    [HttpPost("api/cart")]
    public async Task<IActionResult> AddOrderItem([FromBody] ItemViewModel item)
    {
        var product = await _productAppService.GetByIdAsync(item.Id);

        if (product is null) return BadRequest();

        if (product.QuantityInStock < item.Quantity)
            NotifyError("ValidationError", "Product without a sufficient stock");

        var command = new AddOrderItemCommand(
            CustomerId,
            product.Id,
            product.Name,
            item.Quantity,
            product.Value);

        await _mediator.Send(command);

        return CustomResponse();
    }

    [HttpPut("api/cart/{id:guid}")]
    public async Task<IActionResult> UpdateOrderItem(
        Guid id,
        [FromBody] ItemViewModel item)
    {
        var product = await _productAppService.GetByIdAsync(id);

        if (product is null) return BadRequest();

        var command = new UpdateOrderItemCommand(CustomerId, product.Id, item.Quantity);

        await _mediator.Send(command);

        return CustomResponse();
    }

    [HttpDelete("api/cart/{id:guid}")]
    public async Task<IActionResult> DeleteOrderItem(Guid id)
    {
        var product = await _productAppService.GetByIdAsync(id);

        if (product is null) return BadRequest();

        var command = new RemoveOrderItemCommand(CustomerId, id);

        await _mediator.Send(command);

        return CustomResponse();
    }

    [AllowAnonymous]
    [HttpPost("api/login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel login)
    {
        var result = await _signInManager.PasswordSignInAsync(
            login.Email,
            login.Password,
            false,
            true);

        if (result.Succeeded) return Ok(await GenerateJwtAsync(login.Email));

        NotifyError("Login", "Login or password is incorrect");
        return CustomResponse();
    }

    private async Task<string> GenerateJwtAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var claims = await _userManager.GetClaimsAsync(user);

        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));

        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(claims);

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = _appSettings.Issuer,
            Audience = _appSettings.ValidIn,
            Subject = identityClaims,
            Expires = DateTime.UtcNow.AddHours(_appSettings.ExpirationHours),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        });

        var tokenResult = tokenHandler.WriteToken(token);
        return tokenResult;
    }
}