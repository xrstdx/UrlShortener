namespace UrlShortener.Application.DTOs.UrlRecords
{
    public class UrlRecordDescDTO
    {
        public Guid Id { get; set; }

        public string OriginalUrl { get; set; } = string.Empty;

        public string ShortenedUrl { get; set; } = string.Empty;

        public string CreatedBy { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }
    }
}