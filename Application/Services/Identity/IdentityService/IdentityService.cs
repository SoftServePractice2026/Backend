using Application.Auth;
using Application.Dtos;
using Application.Dtos.Identity;
using AutoMapper;
using Infrastructure.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Services.Identity.IdentityService;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtProvider _jwtProvider;
    private readonly IMapper _mapper;
    private readonly ILogger<IdentityService> _logger;

    public IdentityService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider,IMapper mapper, ILogger<IdentityService> logger)
    {
        _userManager = userManager;
        _jwtProvider = jwtProvider;
        _mapper = mapper;
        _logger = logger;
    }


    public async Task<IdentityDetailsDto> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
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
            throw new Exception(result.Errors.First().Description);
        }
        
        return _mapper.Map<IdentityDetailsDto>(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new Exception("password or email is incorrect");
        }

        var roles = await _userManager.GetRolesAsync(user);
        
        var token = _jwtProvider.GenerateToken(user.Id, roles);
        
        var userDetails = _mapper.Map<IdentityDetailsDto>(user);

        return new AuthResponse(token, DateTime.UtcNow.AddHours(3), userDetails);
    }
}