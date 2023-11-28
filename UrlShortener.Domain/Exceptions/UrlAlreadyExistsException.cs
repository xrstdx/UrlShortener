namespace UrlShortener.Domain.Exceptions
{
    public class UrlAlreadyExistsException : Exception
    {
        public UrlAlreadyExistsException(string message = "Url already exists") : base(message) { }
    }
}
