namespace UrlShortener.Domain.Exceptions
{
    public class UrlRecordNotFoundException : Exception
    {
        public UrlRecordNotFoundException(string message = "Url record not found") : base(message) { }
    }
}
