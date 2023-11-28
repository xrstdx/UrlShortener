using Microsoft.EntityFrameworkCore;
using UrlShortener.Infrastructure.Contexts;

namespace UrlShortener.UnitTests.ServiceTests
{
    public abstract class ServiceTestsBase
    {
        protected static UrlShortenerDbContext CreateContext()
        {
            var builder = new DbContextOptionsBuilder<UrlShortenerDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging();

            return new UrlShortenerDbContext(builder.Options);
        }

        protected static async Task AddItems<T>(UrlShortenerDbContext efContext, params T[] entities) where T : class
        {
            foreach (var entity in entities)
            {
                await efContext.Set<T>().AddAsync(entity);
                await efContext.SaveChangesAsync();
                efContext.Entry(entity).State = EntityState.Detached;
            }
        }
    }
}