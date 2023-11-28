namespace UrlShortener.Domain.Exceptions;

public class UserIdNullOrEmptyException : Exception
{
    public UserIdNullOrEmptyException(string message = "userId is null or empty") : base(message) { }
}
