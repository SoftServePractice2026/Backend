using Application.Dtos;
using Application.Dtos.Identity;
using Shared;
using System.Security.Claims;

namespace Application.Services.Identity;


public interface IIdentityService
{
    Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    Task<Result<IdentityDetailsDto>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
    Task LogoutAsync(string refreshTokenValue);
    Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
    Task<Result<IdentityDetailsDto>> GetCurrentUserAsync(ClaimsPrincipal user);
    
    Task<Result<IdentityDetailsDto>> UpdateUserAsync(Guid UserId, UpdateUserDto request, CancellationToken cancellationToken);
    Task<Result<List<FavoriteMovieResponse>>> GetMyFavoriteMoviesAsync(
        Guid userId, 
        CancellationToken cancellationToken);
            
    Task<Result<FavoriteMovieResponse>> AddFavoriteMovieAsync(
        Guid userId, 
        AddFavoriteMovieRequest request, 
        CancellationToken cancellationToken);
            
    Task DeleteFavoriteMovieAsync(Guid userId, Guid movieId, CancellationToken cancellationToken);
}