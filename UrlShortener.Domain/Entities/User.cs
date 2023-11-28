using UrlShortener.Domain.Enums;

namespace UrlShortener.Domain.Entities;

public class User : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public Roles Role { get; set; } 

    public ICollection<UrlRecord> UrlRecords { get; set; } = new List<UrlRecord>();
}
