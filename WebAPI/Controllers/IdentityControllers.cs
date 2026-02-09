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

        if (result.IsSuccess)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Expires = result.Value.ExpiryDate,
                Path = "/"
            };

            Response.Cookies.Append("access_token", result.Value.Token, cookieOptions);
            return Ok(result.Value);
        }

        return BadRequest(result.Failure);
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
}