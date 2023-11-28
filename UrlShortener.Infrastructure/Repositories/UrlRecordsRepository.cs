using Microsoft.EntityFrameworkCore;
using UrlShortener.Domain.Abstractions.Repositories;
using UrlShortener.Domain.Entities;
using UrlShortener.Infrastructure.Contexts;

namespace UrlShortener.Infrastructure.Repositories;

public class UrlRecordsRepository : IUrlRecordsRepository
{
    private readonly UrlShortenerDbContext _dbContext;

    public UrlRecordsRepository(UrlShortenerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateAsync(UrlRecord urlRecord)
    {
        _dbContext.UrlRecords.Add(urlRecord);

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAllAsync()
    {
        await _dbContext.UrlRecords.ExecuteDeleteAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        _dbContext.UrlRecords.Remove(new UrlRecord { Id = id });

        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<UrlRecord>> GetAll(int amount = 20, int page = 1)
    {
        var urlRecords = await _dbContext.UrlRecords.AsNoTracking()
            .OrderByDescending(ur => ur.CreatedDate)
            .Skip(amount * page)
            .Take(amount)
            .ToListAsync();

        return urlRecords;
    }

    public async Task<UrlRecord?> GetByIdAsync(Guid id)
    {
        var urlRecord = await _dbContext.UrlRecords.AsNoTracking().Include(p => p.User).FirstOrDefaultAsync(ur => ur.Id == id);

        return urlRecord;
    }

    public async Task<string?> GetOriginalUrlByCode(string code)
    {
        var urlRecord = await _dbContext.UrlRecords.FirstOrDefaultAsync(ur => ur.Code == code);

        return urlRecord?.OriginalUrl;
    }

    public async Task<bool> IsCodeExistAsync(string code)
    {
        var codeExists = await _dbContext.UrlRecords.AnyAsync(s => s.Code == code);

        return codeExists;
    }

    public async Task<bool> UrlRecordExistsByOriginalUrlAsync(string originalUrl)
    {
        var recordExists = await _dbContext.UrlRecords.AnyAsync(r => r.OriginalUrl == originalUrl);

        return recordExists;
    }
}
