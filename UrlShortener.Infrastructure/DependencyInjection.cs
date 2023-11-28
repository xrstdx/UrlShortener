using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UrlShortener.Domain.Abstractions.Repositories;
using UrlShortener.Domain.Abstractions.Services;
using UrlShortener.Infrastructure.Contexts;
using UrlShortener.Infrastructure.Providers;
using UrlShortener.Infrastructure.Repositories;
using UrlShortener.Infrastructure.Settings;

namespace UrlShortener.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddJwtAuthentication(configuration);
            services.AddUrlShortenerDbContext(configuration);
            services.AddRepositories();
            services.ConfigureInfrastructureSettings(configuration);

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();

            if (jwtSettings == null)
            {
                throw new ApplicationException("JWT settings are missing in the configuration.");
            }

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                    };
                });

            return services;
        }

        public static IServiceCollection AddUrlShortenerDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UrlShortenerDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IUrlRecordsRepository, UrlRecordsRepository>();
            services.AddScoped<IJwtProvider, JwtProvider>();

            return services;
        }

        public static IServiceCollection ConfigureInfrastructureSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

            return services;
        }
    }
}