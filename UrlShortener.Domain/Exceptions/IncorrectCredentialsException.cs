namespace UrlShortener.Domain.Exceptions
{
    public class IncorrectCredentialsException : Exception
    {
        public IncorrectCredentialsException(string message = "Incorrect email or password") : base(message) { }
    }
}
