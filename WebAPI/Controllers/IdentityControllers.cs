using Application.Dtos.Identity;
using Application.Services.Identity;
using Microsoft.AspNetCore.Mvc;

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
        
        return Ok(result);
    }


    [HttpPost("/login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _identityService.LoginAsync(request, cancellationToken);
        
        return Ok(result);
    }
}