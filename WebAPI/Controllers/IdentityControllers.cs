using Application.Dtos.Identity;
using Application.DTOs.Identity.RecoveryPasswordDtos;
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
    
    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policy.UserPolicy)]
    [HttpGet("favorite")]
    public async Task<IActionResult> GetMyFavorites(CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await _identityService.GetMyFavoriteMoviesAsync(userId, cancellationToken);

        return result.IsFailure
            ? result.Failure!.ToResponse()
            : Ok(result.Value);
    }

    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policy.UserPolicy)]
    [HttpPost("favorite/{movieId:guid}")]
    public async Task<IActionResult> AddToFavorites(Guid movieId, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _identityService.AddFavoriteMovieAsync(
            userId,
            new AddFavoriteMovieRequest { MovieId = movieId },
            cancellationToken);

        return result.IsFailure
            ? result.Failure!.ToResponse()
            : NoContent();
    }

    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policy.UserPolicy)]
    [HttpDelete("favorite/{movieId:guid}")]
    public async Task<IActionResult> RemoveFromFavorites(Guid movieId, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _identityService.DeleteFavoriteMovieAsync(userId, movieId, cancellationToken);
        return NoContent();
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request, CancellationToken cancellationToken)
    {
        var result = await _identityService.ForgotPasswordAsync(request, cancellationToken);

        if (!result.IsSuccess)
            result.Failure!.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var result = await _identityService.ResetPasswordAsync(request, cancellationToken);

        if (!result.IsSuccess)
            return result.Failure!.ToResponse();

        return Ok(result.Value);
    }
}