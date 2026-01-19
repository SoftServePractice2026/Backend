using Application;
using FluentValidation.AspNetCore;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using WebAPI.Filters;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

//Add dependency to custom filter
services.AddScoped<ValidationFailureFilter>();

//Add auto fluent validation on api controllers
services.AddFluentValidationAutoValidation();

services.AddOpenApi();

services.AddDbContext<CinemaDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(CinemaDbContext)));
});

//Add dependency from layers
services
    .AddApplication()
    .AddInfrastructure();


//Add swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "AbsoluteCinema API", Version = "v1" });
});

services.AddControllers(options =>
{
    //Add custom filter
    options.Filters.Add<ValidationFailureFilter>();
});

var app = builder.Build();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
