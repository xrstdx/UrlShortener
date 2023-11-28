using UrlShortener.Domain.Entities;

namespace UrlShortener.Domain.Abstractions.Services;

public interface IJwtProvider
{
    string GenerateToken(User user);
}
