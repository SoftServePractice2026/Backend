using Application.Auth;
using Application.Dtos;
using Application.Dtos.Identity;
using Application.Services.Identity.IdentityService;
using AutoMapper;
using Domain.Constants;
using Infrastructure.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Moq;
using Shared;
using Xunit;
using FluentAssertions;

namespace SoftServePractice2026.Backend.Tests.Services;

public class IdentityServiceTests
{
    private readonly Mock<UserManager<ApplicationUser>> _userManager;
    private readonly Mock<RoleManager<ApplicationRole>> _roleManager;
    private readonly Mock<IJwtProvider> _jwtProvider;
    private readonly Mock<IMapper> _mapper;

    public IdentityServiceTests()
    {
        _userManager = new Mock<UserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(),
            null, null, null, null, null, null, null, null
        );

        _roleManager = new Mock<RoleManager<ApplicationRole>>(
            Mock.Of<IRoleStore<ApplicationRole>>(),
            null, null, null, null
        );

        _jwtProvider = new Mock<IJwtProvider>();
        _mapper = new Mock<IMapper>();
    }

    private IdentityService CreateService()
        => new(
            _userManager.Object,
            _roleManager.Object,
            _jwtProvider.Object,
            _mapper.Object
        );


    [Fact]
    public async Task RegisterAsync_ShouldReturnSuccess_WhenUserNotExists()
    {
        var request = new RegisterRequest(
            "test@mail.com",
            "password1",
            "Max",
            "Parker",
            new DateTime(2000, 1, 1)
        );

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            BirthDate = request.BirthDate
        };

        _userManager
            .Setup(u => u.FindByEmailAsync(request.Email))
            .ReturnsAsync((ApplicationUser)null);

        _mapper
            .Setup(m => m.Map<ApplicationUser>(request))
            .Returns(user);

        _roleManager.Setup(r => r.RoleExistsAsync(Role.User))
            .ReturnsAsync(true);

        _roleManager.Setup(r => r.FindByNameAsync(Role.User))
            .ReturnsAsync(new ApplicationRole
            {
                Name = Role.User
            });


        _userManager
            .Setup(u => u.CreateAsync(user, request.Password))
            .ReturnsAsync(IdentityResult.Success);

        _userManager
            .Setup(u => u.AddToRoleAsync(user, Role.User))
            .ReturnsAsync(IdentityResult.Success);

        _userManager
            .Setup(u => u.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { Role.User }); _jwtProvider
            .Setup(j => j.GenerateToken(user.Id, It.IsAny<IEnumerable<string>>()))
            .Returns("jwt-token");

        var service = CreateService();

        var result = await service.RegisterAsync(request, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Token.Should().Be("jwt-token");
        result.Value.UserDetails.Email.Should().Be(request.Email);
    }

    [Fact]
    public async Task RegisterAsync_ShouldFail_WhenUserAlreadyExists()
    {
        var request = new RegisterRequest(
            "test@mail.com",
            "password1",
            "Max",
            "Parker",
            new DateTime(2000, 1, 1)
        );

        _userManager
            .Setup(u => u.FindByEmailAsync(request.Email))
            .ReturnsAsync(new ApplicationUser());

        var service = CreateService();

        var result = await service.RegisterAsync(request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Failure.Should().NotBeNull();
        result.Failure!.Errors.Should()
            .Contain(e => e.Code == "user.already.exist");
    }



    [Fact]
    public async Task LoginAsync_ShouldReturnToken_WhenCredentialsValid()
    {
        var request = new LoginRequest(
            "test@mail.com",
            "Password123!"
        );

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            FirstName = "Max",
            LastName = "Test"
        };

        _userManager
            .Setup(u => u.FindByEmailAsync(request.Email))
            .ReturnsAsync(user);

        _userManager
            .Setup(u => u.CheckPasswordAsync(user, request.Password))
            .ReturnsAsync(true);

        _userManager
            .Setup(u => u.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { Role.User });

        _jwtProvider
            .Setup(j => j.GenerateToken(user.Id, It.IsAny<IEnumerable<string>>()))
            .Returns("jwt-token");

        var service = CreateService();

        var result = await service.LoginAsync(request, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Token.Should().Be("jwt-token");
    }

    [Fact]
    public async Task LoginAsync_ShouldFail_WhenInvalidCredentials()
    {
        var request = new LoginRequest(
            "test@mail.com",
            "wrong-password"
        );

        _userManager
            .Setup(u => u.FindByEmailAsync(request.Email))
            .ReturnsAsync((ApplicationUser)null);

        var service = CreateService();

        var result = await service.LoginAsync(request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Failure.Errors.Should().Contain(e => e.Code == "Invalid credentials");
    }
}
