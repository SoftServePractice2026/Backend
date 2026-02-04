using Application;
using FluentValidation.AspNetCore;
using Infrastructure;
using Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;
using NSwag;
using NSwag.Generation.Processors.Security;
using WebAPI.Filters;
using WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// add override appsettings.json from appsettings.Development.Local.json
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.Local.json",
        optional: true,
        reloadOnChange: true);

var services = builder.Services;

//Add dependency to custom filter
services.AddScoped<ValidationFailureFilter>();

//Add auto fluent validation on api controllers
services.AddFluentValidationAutoValidation();

//Add dependency from layers
services
    .AddApplication()
    .AddInfrastructure(services, builder.Configuration);


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
    //Add custom filter
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

//Checking is runnig database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();

    if (!await dbContext.Database.CanConnectAsync())
    {
        throw new InvalidOperationException("Cannot connect to database. Check connection string and database availability.");
    }
}

//Seeding roles
using (var scope = app.Services.CreateScope())
{
    await IdentitySeed.SeedRolesAsync(scope.ServiceProvider);
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
