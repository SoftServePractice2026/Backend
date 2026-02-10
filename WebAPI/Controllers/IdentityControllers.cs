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

        if (result.IsFailure)
        {
            return BadRequest(result.Failure);
        }

        var cookieRefresh = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = result.Value!.ExpiryDateRefresh,
            Path = "/",
        };

        Response.Cookies.Append("refreshToken", result.Value!.RefreshToken!, cookieRefresh);

        return Ok(result.Value);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policy.UserPolicy)]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateMyProfile(
        [FromBody] UpdateUserDto request,
        CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await _identityService.UpdateUserAsync(userId, request, cancellationToken);

        return result.IsFailure
            ? result.Failure!.ToResponse()
            : Ok(result.Value);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var refreshTokenValue = Request.Cookies["refreshToken"];

        if (refreshTokenValue != null)
        {
            await _identityService.LogoutAsync(refreshTokenValue);
            Response.Cookies.Delete("refreshToken");
        }

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

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken(CancellationToken cancellationToken)
    {
        var refreshToken = Request.Cookies["refreshToken"];

        if (refreshToken == null)
            return Unauthorized();

        var result = await _identityService
            .RefreshTokenAsync(refreshToken, cancellationToken);

        if (result.IsFailure)
            return Unauthorized(result.Failure);

        var cookieRefresh = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = result.Value!.ExpiryDateRefresh,
            Path = "/",
        };

        Response.Cookies.Append("refreshToken", result.Value!.RefreshToken!, cookieRefresh);

        return Ok(result.Value);
    }
}