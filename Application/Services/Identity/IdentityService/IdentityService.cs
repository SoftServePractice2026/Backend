using Application.Auth;
using Application.Dtos;
using Application.Dtos.Identity;
using AutoMapper;
using CSharpFunctionalExtensions;
using Infrastructure.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Services.Identity.IdentityService;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtProvider _jwtProvider;
    private readonly IMapper _mapper;

    public IdentityService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider,IMapper mapper)
    {
        _userManager = userManager;
        _jwtProvider = jwtProvider;
        _mapper = mapper;
    }


    public async Task<Result<IdentityDetailsDto, Failure>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            BirthDate = request.BirthDate
        };
        
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return Failure.FromError(Error.Validation("Registration failed", "Registration failed."));
        }
        
        var detailsDto = _mapper.Map<IdentityDetailsDto>(user);
        
        return Result.Success<IdentityDetailsDto, Failure>(detailsDto);
    }

    public async Task<Result<AuthResponse, Failure>> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return Failure.FromError(Error.Unauthorized("Invalid credentials", "Invalid credentials."));
        }

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtProvider.GenerateToken(user.Id, roles);
        var userDetails = _mapper.Map<IdentityDetailsDto>(user);

        var response = new AuthResponse(token, DateTime.UtcNow.AddHours(3), userDetails);
        
        return Result.Success<AuthResponse, Failure>(response);
    }

    public async Task LogoutAsync()
    {
        await Task.CompletedTask;
    }

    public async Task<Result<AuthResponse, Failure>> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken, cancellationToken);
        
        
        if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return Failure.FromError(Error.Unauthorized("Invalid token", "Invalid token."));
        }
        
        var roles = await _userManager.GetRolesAsync(user);
        var newJwtToken = _jwtProvider.GenerateToken(user.Id, roles);
        var newRefreshToken = _jwtProvider.GenerateRefreshToken();
        
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        
        await _userManager.UpdateAsync(user);

        var userDetails = _mapper.Map<IdentityDetailsDto>(user);
        var response = new AuthResponse(newJwtToken, DateTime.UtcNow.AddHours(3), userDetails);
        
        return Result.Success<AuthResponse, Failure>(response);
    }
}