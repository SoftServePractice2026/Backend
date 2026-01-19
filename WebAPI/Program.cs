using Application;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using WebAPI.Filters;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddControllers(options =>
{
    options.Filters.Add<ValidationFailureFilter>();
});


services.AddOpenApi();

services.AddDbContext<CinemaDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(CinemaDbContext)));
});

services
    .AddApplication()
    .AddInfrastructure();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
