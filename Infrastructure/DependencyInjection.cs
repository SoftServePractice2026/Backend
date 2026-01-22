using Domain.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {

        //Dependency repositories
        services.AddScoped<IHallRepository, HallRepository>();

        services.AddScoped<IActorRepository, ActorRepository>();

        return services;
    }
}