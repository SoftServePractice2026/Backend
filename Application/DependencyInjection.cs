using Application.Services;
using Application.Services.Hall;
using Application.Services.Identity;
using Application.Services.Identity.IdentityService;
using Application.Services.Movie.MovieService;
using Application.Validators.Halls;
using Domain.Interfaces;
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
       
        //Dependency all mapping profiles in current assembly
        services.AddAutoMapper(cfg => { }, currentAssembly);

        //Dependency services
        services.AddScoped<IHallService, HallService>();
        services.AddScoped<IActorService, ActorService>();
        services.AddScoped<IGenreService, GenreService>();
        services.AddScoped<ITicketService, TicketService>();
        services.AddScoped<IViewHistoryService, ViewHistoryService>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IMovieService, MovieService>();

        return services;
    }
}