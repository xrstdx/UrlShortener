using UrlShortener.Application.DTOs.UrlRecords;

namespace UrlShortener.Application.Abstractions.Services
{
    public interface IUrlRecordsService
    {
        Task CreateAsync(string? userId, CreateUrlRecordDTO model);
        Task DeleteByIdAsync(string? userId, Guid urlRecordId);
        Task DeleteAllAsync();
        Task<List<UrlRecordDTO>> GetAllAsync(int amount, int page);
        Task<UrlRecordDescDTO> GetByIdAsync(Guid id);
        Task<string> GetOriginalUrlByCodeAsync(string code);
    }
}
