using UrlShortener.API.Middlewares;
using UrlShortener.Infrastructure;
using UrlShortener.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

builder.Services.AddControllers();

var configurations = builder.Configuration;

builder.Services
    .AddApplication(configurations)
    .AddInfrastructure(configurations);

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors(options =>
     options.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandler>();

app.MapControllers();

app.Run();
