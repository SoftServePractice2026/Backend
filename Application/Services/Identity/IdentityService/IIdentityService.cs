using Application.Dtos;
using Application.Dtos.Identity;
using CSharpFunctionalExtensions;

namespace Application.Services.Identity;

public interface IIdentityService
{
    Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    
    Task<IdentityDetailsDto> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
    
}