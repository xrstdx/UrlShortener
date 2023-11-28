namespace UrlShortener.Domain.Exceptions
{
    public class AccessForbiddenException : Exception
    {
        public AccessForbiddenException(string message = "Access Forbidden") : base(message) { }
    }
}
