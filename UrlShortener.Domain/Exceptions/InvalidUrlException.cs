namespace UrlShortener.Domain.Exceptions;

public class InvalidUrlException : Exception
{
    public InvalidUrlException(string message = "Url is invalid") : base(message) { }
}
