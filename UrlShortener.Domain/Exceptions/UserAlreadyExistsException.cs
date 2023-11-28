namespace UrlShortener.Domain.Exceptions;

public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException(string message = "User with given email already exists.") : base(message) { }
}
