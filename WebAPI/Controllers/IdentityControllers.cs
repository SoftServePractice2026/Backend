using Application.Dtos.Identity;
using Application.Services.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebAPI.ResponseExtensions;

namespace WebAPI.Controllers;

public class IdentityControllers : ControllerBase
{
    private readonly IIdentityService _identityService;

    public IdentityControllers(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost("/register")]

    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _identityService.RegisterAsync(request, cancellationToken);
        
        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }


    [HttpPost("/login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _identityService.LoginAsync(request, cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }


    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _identityService.LogoutAsync();

        return Ok(new { message = "Logout successful" });
    }
}