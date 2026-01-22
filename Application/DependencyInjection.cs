using Application.Services;
using Application.Validators.Actors;
using Application.Validators.Halls;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var currentAssembly = typeof(DependencyInjection).Assembly;

        //Dependency all validators in current assembly
        services.AddValidatorsFromAssemblyContaining<HallCreateDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<ActorCreateDtoValidator>();

        //Dependency all mapping profiles in current assembly
        services.AddAutoMapper(cfg => { }, currentAssembly);
        services.AddAutoMapper(acg => { }, currentAssembly);

        //Dependency services
        services.AddScoped<IHallService, HallService>();
        services.AddScoped<IActorService, ActorService>();

        return services;
    }
}