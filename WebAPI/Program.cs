using Application;
using Application.Interfaces;
using Application.Services;
using Application.Services.ExternalMovie;
using FluentValidation.AspNetCore;
using Infrastructure;
using Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;
using NSwag;
using NSwag.Generation.Processors.Security;
using WebAPI.Filters;
using WebAPI.Middlewares;
var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.Local.json",
        optional: true,
        reloadOnChange: true);

var services = builder.Services;

services.AddScoped<ValidationFailureFilter>();
services.AddFluentValidationAutoValidation();

//Add dependency from layers
services
    .AddApplication()
    .AddInfrastructure(services, builder.Configuration);

services.AddHttpClient<IExternalMovieService, TMDBService>();

//Add swagger
builder.Services.AddOpenApiDocument(opt =>
{
    opt.AddSecurity("Bearer", new OpenApiSecurityScheme
    {
        Description = "Bearer token authorization header",
        Type = OpenApiSecuritySchemeType.Http,
        In = OpenApiSecurityApiKeyLocation.Header,
        Name = "Authorization",
        Scheme = "Bearer"
    });

    opt.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
});

services.AddControllers(options =>
{
    options.Filters.Add<ValidationFailureFilter>();
});

var frontendOrigins = builder.Configuration.GetSection("Cors").GetSection("AllowedOrigins").Get<string[]>();
services.AddCors(opt =>
{
    opt.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .WithOrigins(frontendOrigins!)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

var app = builder.Build();

app.UseCors("FrontendPolicy");

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();
    var serviceProvider = scope.ServiceProvider;

    if (!await dbContext.Database.CanConnectAsync())
    {
        throw new InvalidOperationException("Cannot connect to database. Check connection string and database availability.");
    }

    try
    {
        //Domain seed
        var seeder = serviceProvider.GetRequiredService<HallSeed>();
        var seeder1 = serviceProvider.GetRequiredService<SessionSeed>();
        await seeder.SeedHallsAndSeatsAsync();
        await seeder1.SeedAsync();
        //Identity seed
        await IdentitySeed.SeedRolesAsync(scope.ServiceProvider);
    }
    catch (Exception ex)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.UseMiddleware<LoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();