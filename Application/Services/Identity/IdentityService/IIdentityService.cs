using Application.Dtos;
using Application.Dtos.Identity;
using Shared;
using System.Security.Claims;

namespace Application.Services.Identity;

public interface IIdentityService
{
    Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
    Task LogoutAsync();
    Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
    Task<Result<IdentityDetailsDto>> GetCurrentUserAsync(ClaimsPrincipal user);
}