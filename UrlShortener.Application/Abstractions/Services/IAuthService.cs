using UrlShortener.Application.DTOs.Auth;

namespace UrlShortener.Application.Abstractions.Services;

public interface IAuthService
{
    Task RegisterAsync(RegistrationDTO model);

    Task<string> LoginAsync(LoginDTO model);
}
