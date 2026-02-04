using Application;
using FluentValidation.AspNetCore;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using WebAPI.Filters;
using WebAPI.Middlewares;
using NSwag;
using NSwag.Generation.Processors.Security;

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

//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new() { Title = "AbsoluteCinema API", Version = "v1" });
//});

services.AddControllers(options =>
{
    //Add custom filter
    options.Filters.Add<ValidationFailureFilter>();
});

services.AddCors(opt =>
{
    opt.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
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
