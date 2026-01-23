using Application.Dtos;
using Application.Dtos.Identity;
using CSharpFunctionalExtensions;
using Shared;

namespace Application.Services.Identity;

public interface IIdentityService
{
    Task<Result<AuthResponse, Failure>> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    
    Task<Result<IdentityDetailsDto, Failure>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
    
    Task LogoutAsync();

    Task<Result<AuthResponse, Failure>> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
}