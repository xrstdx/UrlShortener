using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Infrastructure.EntityConfigurations;

public class UrlRecordConfiguration : IEntityTypeConfiguration<UrlRecord>
{
    public void Configure(EntityTypeBuilder<UrlRecord> builder)
    {
        builder.ToTable("UrlRecords");

        builder.HasKey(e => e.Id);

        builder.Property(ur => ur.OriginalUrl).IsRequired();
        builder.Property(ur => ur.ShortenedUrl).IsRequired();
        builder.Property(ur => ur.Code).IsRequired();
        builder.Property(ur => ur.CreatedDate).IsRequired();
    }
}
