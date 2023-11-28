namespace UrlShortener.Domain.Entities;

public class UrlRecord : BaseEntity
{
    public string OriginalUrl { get; set; } = string.Empty;
    public string ShortenedUrl { get; set; } = string.Empty;
    public string Code { get;set; } = string.Empty;
    public DateTime CreatedDate { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }
}