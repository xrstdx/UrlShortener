namespace UrlShortener.Domain.Exceptions;

public class UrlNotFoundException : Exception
{
    public UrlNotFoundException(string message = "Url not found") : base(message) { }
}
