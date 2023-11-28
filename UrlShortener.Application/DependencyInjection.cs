using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Application.Abstractions.Services;
using UrlShortener.Application.Services;
using UrlShortener.Application.Settings;

namespace UrlShortener.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddServices();
        services.ConfigureApplicationSettings(configuration);

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUrlRecordsService, UrlRecordsService>();
        services.AddScoped<IUrlShorteningService, UrlShorteningService>();

        return services;
    }

    public static IServiceCollection ConfigureApplicationSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<HostSettings>(configuration.GetSection("HostSettings"));

        return services;
    }
}