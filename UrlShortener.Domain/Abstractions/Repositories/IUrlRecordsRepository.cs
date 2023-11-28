using UrlShortener.Domain.Entities;

namespace UrlShortener.Domain.Abstractions.Repositories;

public interface IUrlRecordsRepository
{
    Task<UrlRecord?> GetByIdAsync(Guid id);
    Task DeleteAllAsync();
    Task DeleteAsync(Guid id);
    Task<List<UrlRecord>> GetAll(int amount, int page);
    Task<bool> UrlRecordExistsByOriginalUrlAsync(string originalUrl);
    Task<bool> IsCodeExistAsync(string code);
    Task CreateAsync(UrlRecord urlRecord);
    Task<string?> GetOriginalUrlByCode(string code);
}
