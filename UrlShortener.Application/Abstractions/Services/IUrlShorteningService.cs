namespace UrlShortener.Application.Abstractions.Services;

public interface IUrlShorteningService
{
    Task<string> GetShortUrl();
}
