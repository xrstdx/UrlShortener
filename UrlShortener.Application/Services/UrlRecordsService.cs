using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UrlShortener.Application.Abstractions.Services;
using UrlShortener.Application.DTOs.UrlRecords;
using UrlShortener.Application.Settings;
using UrlShortener.Domain.Abstractions.Repositories;
using UrlShortener.Domain.Entities;
using UrlShortener.Domain.Enums;
using UrlShortener.Domain.Exceptions;

namespace UrlShortener.Application.Services;

public class UrlRecordsService : IUrlRecordsService
{
    private readonly IUrlRecordsRepository _urlRecordsRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IUrlShorteningService _urlShorteningService;
    private readonly HostSettings _hostSettings;

    public UrlRecordsService(IUrlRecordsRepository urlRecordsRepository, IUsersRepository usersRepository, IUrlShorteningService urlShorteningService, IOptions<HostSettings> options)
    {
        _urlRecordsRepository = urlRecordsRepository;
        _usersRepository = usersRepository;
        _urlShorteningService = urlShorteningService;
        _hostSettings = options.Value;
    }

    public async Task CreateAsync(string? userId, CreateUrlRecordDTO model)
    {
        if (userId.IsNullOrEmpty()) 
            throw new UserIdNullOrEmptyException();

        if (!Uri.TryCreate(model.OriginalUrl, UriKind.Absolute, out _))
            throw new InvalidUrlException();

        var isExists = await _urlRecordsRepository.UrlRecordExistsByOriginalUrlAsync(model.OriginalUrl);

        if (isExists)
            throw new UrlAlreadyExistsException();

        var code = await _urlShorteningService.GetShortUrl();

        var urlRecord = new UrlRecord()
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Parse(userId!),
            OriginalUrl = model.OriginalUrl,
            Code = code,
            ShortenedUrl = $"{_hostSettings.Scheme}://{_hostSettings.Host}/r/{code}",
            CreatedDate = DateTime.UtcNow
        };      

        await _urlRecordsRepository.CreateAsync(urlRecord);
    }

    public async Task DeleteByIdAsync(string? userId, Guid urlRecordId)
    {
        if (userId.IsNullOrEmpty())
            throw new AccessForbiddenException();

        var parcedUserId = Guid.Parse(userId!);

        var user = await _usersRepository.GetUserByIdAsync(parcedUserId) ?? throw new AccessForbiddenException();

        var urlRecord = await _urlRecordsRepository.GetByIdAsync(urlRecordId) ?? throw new UrlRecordNotFoundException();

        if (user.Role != Roles.Admin || urlRecord.UserId != user.Id)
            throw new AccessForbiddenException();

        await _urlRecordsRepository.DeleteAsync(urlRecordId);
    }

    public async Task DeleteAllAsync()
    {
        await _urlRecordsRepository.DeleteAllAsync();
    }
    
    public async Task<UrlRecordDescDTO> GetByIdAsync(Guid id)
    {
        var urlRecord = await _urlRecordsRepository.GetByIdAsync(id) ?? throw new UrlRecordNotFoundException();

        var urlRecordDTO = new UrlRecordDescDTO
        {
            Id = urlRecord.Id,
            OriginalUrl = urlRecord.OriginalUrl,
            ShortenedUrl = urlRecord.ShortenedUrl,
            CreatedBy = urlRecord.User.FullName,
            CreatedDate = urlRecord.CreatedDate
        };

        return urlRecordDTO;
    }

    public async Task<List<UrlRecordDTO>> GetAllAsync(int amount, int page)
    {
        var urlRecords = await _urlRecordsRepository.GetAll(amount, page);

        var urlRecordsDTO = urlRecords.Select(p => new UrlRecordDTO
        {
            Id = p.Id,
            OriginalUrl = p.OriginalUrl,
            ShortenedUrl = p.ShortenedUrl
        }).ToList();

        return urlRecordsDTO;
    }

    public async Task<string> GetOriginalUrlByCodeAsync(string code)
    {
        var originalUrl = await _urlRecordsRepository.GetOriginalUrlByCode(code) ?? throw new UrlNotFoundException();

        return originalUrl;
    }
}