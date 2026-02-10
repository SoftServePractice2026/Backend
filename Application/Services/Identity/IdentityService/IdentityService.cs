using Application.Auth;
using Application.Dtos;
using Application.Dtos.Identity;
using AutoMapper;
using Domain.Constants;
using Infrastructure.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared;
using System.Data;
using System.Security.Claims;
using Application.Services.Movie.MovieRepository;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services.Identity.IdentityService;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IJwtProvider _jwtProvider;
    private readonly IMapper _mapper;
    
    private readonly IFavoriteMovieRepository _favoriteMovieRepository;
    private readonly IMovieRepository _movieRepository;
    private readonly IUnitOfWork _unitOfWork;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IJwtProvider jwtProvider,
        IMapper mapper,
        IFavoriteMovieRepository favoriteMovieRepository,
        IMovieRepository movieRepository,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtProvider = jwtProvider;
        _mapper = mapper;

        _favoriteMovieRepository = favoriteMovieRepository;
        _movieRepository = movieRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IdentityDetailsDto>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        var userExist = await _userManager.FindByEmailAsync(request.Email);

        if (userExist is not null)
        {
            return Result<IdentityDetailsDto>.Fail(Error.Conflict("user.already.exist", $"User with email: {request.Email} already exist"));
        }

        var user = _mapper.Map<ApplicationUser>(request);

        var userRole = Role.User;
        if (!await _roleManager.RoleExistsAsync(userRole))
        {
            return Result<IdentityDetailsDto>.Fail(Error.Internal("user.role.not.found", "User role not found"));
        }

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => Error.Validation(e.Code, e.Description));
            return Result<IdentityDetailsDto>.Fail(new Failure(errors));
        }

        var roleEntity = await _roleManager.FindByNameAsync(userRole);
        var addRoleResult = await _userManager.AddToRoleAsync(user, roleEntity!.Name!);

        var userRoles = await _userManager.GetRolesAsync(user);

        var detailsDto = new IdentityDetailsDto(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email!,
                user.BirthDate,
                userRoles);

        return Result<IdentityDetailsDto>.Success(detailsDto);
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return Result<AuthResponse>.Fail(Error.Unauthorized("Invalid credentials", "Invalid password or email."));
        }

        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _jwtProvider.GenerateToken(user.Id, roles);
        var refreshToken = _jwtProvider.GenerateRefreshToken();

        user.RefreshToken = refreshToken.token;
        var refreshTokenExpiry = DateTime.UtcNow.AddMinutes(refreshToken.expiry);
        user.RefreshTokenExpiryTime = refreshTokenExpiry;
        await _userManager.UpdateAsync(user);

        var userDetails = new IdentityDetailsDto(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email!,
                user.BirthDate,
                roles);

        var response = new AuthResponse(
            accessToken.token,
            DateTime.UtcNow.AddMinutes(accessToken.expiry),
            userDetails,
            refreshToken.token,
            refreshTokenExpiry);

        return Result<AuthResponse>.Success(response);
    }

    public async Task LogoutAsync(string refreshTokenValue)
    {
        if (!string.IsNullOrEmpty(refreshTokenValue))
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshTokenValue);

            if (user != null)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = DateTime.UtcNow;

                await _userManager.UpdateAsync(user);
            }
        }
    }

    public async Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken, cancellationToken);

        if (user == null)
        {
            return Result<AuthResponse>.Fail(Error.Unauthorized("Invalid token", "Invalid token."));

        }

        if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return Result<AuthResponse>.Fail(Error.Unauthorized("Invalid token", "Refresh token expired."));
        }

        var roles = await _userManager.GetRolesAsync(user);
        var newJwtToken = _jwtProvider.GenerateToken(user.Id, roles);
        var newRefreshToken = _jwtProvider.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken.token;
        var refreshTokenExpiry = DateTime.UtcNow.AddMinutes(newRefreshToken.expiry);
        user.RefreshTokenExpiryTime = refreshTokenExpiry;

        await _userManager.UpdateAsync(user);

        var userDetails = new IdentityDetailsDto(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email!,
                user.BirthDate,
                roles);

        var response = new AuthResponse(
            newJwtToken.token,
            DateTime.UtcNow.AddMinutes(newJwtToken.expiry),
            userDetails,
            newRefreshToken.token,
            refreshTokenExpiry);

        return Result<AuthResponse>.Success(response);
    }

    public async Task<Result<IdentityDetailsDto>> GetCurrentUserAsync(ClaimsPrincipal principal)
    {
        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            return Result<IdentityDetailsDto>.Fail(Error.Unauthorized("unauthorized", "User is not authenticated"));
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result<IdentityDetailsDto>.Fail(Error.NotFound("user_not_found", "User does not exist"));
        }

        var roles = await _userManager.GetRolesAsync(user);


        return Result<IdentityDetailsDto>.Success(
            new IdentityDetailsDto(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email!,
                user.BirthDate,
                roles
            )
        );
    }

    public async Task<Result<IdentityDetailsDto>> UpdateUserAsync(Guid userId, UpdateUserDto request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            return Result<IdentityDetailsDto>.Fail(Error.NotFound("user.not.found", $"User with id: {userId} not found"));
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.BirthDate = request.BirthDate;
        user.PhoneNumber = request.PhoneNumber;
        
        var result = await _userManager.UpdateAsync(user);
        
        if (!result.Succeeded)
        {
            return Result<IdentityDetailsDto>.Fail(Error.Validation(
                "user.update.failed", 
                "User update failed"));
        }
        
        var roles = await _userManager.GetRolesAsync(user);

        return Result<IdentityDetailsDto>.Success(
            new IdentityDetailsDto(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email!,
                user.BirthDate,
                roles
            ));
    }
    
    
    public async Task<Result<List<FavoriteMovieResponse>>> GetMyFavoriteMoviesAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return Result<List<FavoriteMovieResponse>>.Fail(
                Error.NotFound("user.not.found", $"User with id: {userId} not found")
            );
        }

        var movieIds = await _favoriteMovieRepository.GetMovieIdsByUserAsync(userId, cancellationToken);

        var response = movieIds.Select(movieId => new FavoriteMovieResponse
        {
            FavoriteId = Guid.Empty,
            MovieId = movieId,
            UserId = userId,
            AddedAt = default
        }).ToList();

        return Result<List<FavoriteMovieResponse>>.Success(response);
    }

    public async Task<Result<FavoriteMovieResponse>> AddFavoriteMovieAsync(
        Guid userId,
        AddFavoriteMovieRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return Result<FavoriteMovieResponse>.Fail(
                Error.NotFound("user.not.found", $"User with id: {userId} not found")
            );
        }

        var movie = await _movieRepository.GetMovieByIdAsync(request.MovieId, cancellationToken);

        if (movie is null)
        {
            return Result<FavoriteMovieResponse>.Fail(
                Error.NotFound("movie.not.found", $"Movie with id: {request.MovieId} not found")
            );
        }

        var alreadyExists = await _favoriteMovieRepository.ExistsAsync(userId, request.MovieId, cancellationToken);
        if (alreadyExists)
        {
            return Result<FavoriteMovieResponse>.Fail(
                Error.Conflict("favorite.already.exists", "Movie is already in favorites")
            );
        }

        var entity = new FavoriteMovieEntity
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            MovieId = request.MovieId,
            AddedAt = DateTime.Now
        };

        _favoriteMovieRepository.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        
        return Result<FavoriteMovieResponse>.Success(new FavoriteMovieResponse
        {
            FavoriteId = entity.Id,
            MovieId = entity.MovieId,
            UserId = entity.UserId,
            AddedAt = entity.AddedAt
        });
    }

    
    public async Task DeleteFavoriteMovieAsync(Guid userId, Guid movieId, CancellationToken cancellationToken)
    {
        var favorite = await _favoriteMovieRepository.GetByUserAndMovieAsync(userId, movieId, cancellationToken);
        if (favorite is null)
        {
            return;
        }
        _favoriteMovieRepository.Remove(favorite);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }


}