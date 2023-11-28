namespace UrlShortener.Application.DTOs.UrlRecords
{
    public class UrlRecordDTO
    {
        public Guid Id { get; set; }

        public string OriginalUrl { get; set; } = string.Empty;

        public string ShortenedUrl { get; set; } = string.Empty;
    }
}