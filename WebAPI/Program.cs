using Application;
using Application.Interfaces;
using Application.Services;
using Application.Services.ExternalMovie;
using FluentValidation.AspNetCore;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
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

services.AddOpenApi();

services
    .AddApplication()
    .AddInfrastructure(services, builder.Configuration);


services.AddHttpClient<IExternalMovieService, TMDBService>();

services.AddScoped<IMovieImportService, MovieImportService>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "AbsoluteCinema API", Version = "v1" });
});

services.AddControllers(options =>
{
    options.Filters.Add<ValidationFailureFilter>();
});

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();

    if (!await dbContext.Database.CanConnectAsync())
    {
        throw new InvalidOperationException("Cannot connect to database. Check connection string and database availability.");
    }
}

app.UseMiddleware<LoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AbsoluteCinema API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();