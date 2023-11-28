using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Infrastructure.Contexts
{
    public class UrlShortenerDbContext : DbContext
    {
        public UrlShortenerDbContext(DbContextOptions<UrlShortenerDbContext> options) : base(options){}
        public DbSet<User> Users { get; set; }
        public DbSet<UrlRecord> UrlRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
