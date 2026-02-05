using Application.Dtos.Identity;
using Application.Services.Identity;
using Domain.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.ResponseExtensions;

namespace WebAPI.Controllers;

[Route("/api/v1")]
public class IdentityControllers : BaseController
{
    private readonly IIdentityService _identityService;

    public IdentityControllers(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [AllowAnonymous]
    [HttpPost("registration")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _identityService.RegisterAsync(request, cancellationToken);
        
        return result.IsFailure
            ? result.Failure!.ToResponse()
            : Ok(result.Value);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _identityService.LoginAsync(request, cancellationToken);

        return result.IsFailure
            ? result.Failure!.ToResponse()
            : Ok(result.Value);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policy.UserPolicy)]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _identityService.LogoutAsync();

        return Ok(new { message = "Logout successful" });
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policy.UserPolicy)]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var result = await _identityService.GetCurrentUserAsync(User);

        if (!result.IsSuccess)
            return Unauthorized(result.Failure);

        return Ok(result.Value);
    }
}