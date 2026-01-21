using Application.Auth;
using Infrastructure.Authentication;
using Infrastructure.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IServiceCollection serviceCollection, IConfiguration configuration)
    {
        services.AddDbContext<CinemaDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString(nameof(CinemaDbContext)));
        });

        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<CinemaDbContext>()
            .AddDefaultTokenProviders();
        
        
        services.AddScoped<IJwtProvider, JwtProvider>();

        // services.Configure<JwtOptions>(configuration.GetSection());
        
        return services;
    }
}