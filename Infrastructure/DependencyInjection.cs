using System.Text;
using Application.Auth;
using Application.Services.Movie.MovieRepository;
using Infrastructure.Authentication;
using Infrastructure.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Domain.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Domain.Constants;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(nameof(CinemaDbContext));

        if(string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException($"Connection string {nameof(CinemaDbContext)} is missing in appsettings.json or environment variables.");
        }

        services.AddDbContext<CinemaDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
            })
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<CinemaDbContext>()
            .AddDefaultTokenProviders();

        
        services.AddOptions<JwtOptions>()
            .Bind(configuration.GetSection("JwtOptions"))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        
        var jwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>() 
                         ?? throw new InvalidOperationException("JWT configuration is missing.");

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                };

                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["access_token"];

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization(opt =>
        {
            opt.AddPolicy(Policy.UserPolicy, policy =>
            {
                policy.RequireRole(Role.User, Role.Admin);
            });

            opt.AddPolicy(Policy.AdminPolicy, policy =>
            {
                policy.RequireRole(Role.Admin);
            });
        });

        services.AddScoped<IJwtProvider, JwtProvider>();
        
        //Dependency repositories
        services.AddScoped<IHallRepository, HallRepository>();
        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IActorRepository, ActorRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<ISeatRepository, SeatRepository>();
        services.AddScoped<IViewHistoryRepository, ViewHistoryRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}