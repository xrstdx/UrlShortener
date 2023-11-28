using System.Text;
using UrlShortener.Application.Abstractions.Services;
using UrlShortener.Domain.Abstractions.Repositories;

namespace UrlShortener.Application.Services
{
    public class UrlShorteningService : IUrlShorteningService
    {
        private const int ShortUrlLength = 8;
        private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        private readonly Random _random = new();
        private readonly IUrlRecordsRepository _urlRecordsRepository;

        public UrlShorteningService(IUrlRecordsRepository urlRecordsRepository)
        {
            _urlRecordsRepository = urlRecordsRepository;
        }

        public async Task<string> GetShortUrl()
        {
            const int maxAttempts = 10;

            for (var attempt = 0; attempt < maxAttempts; attempt++)
            {
                var code = GenerateRandomCode();

                var isExist = await _urlRecordsRepository.IsCodeExistAsync(code);

                if (!isExist)
                {
                    return code;
                }
            }

            throw new Exception();
        }

        private string GenerateRandomCode()
        {
            var sb = new StringBuilder(ShortUrlLength);

            for (var i = 0; i < ShortUrlLength; i++)
            {
                var randomIndex = _random.Next(Alphabet.Length);

                sb.Append(Alphabet[randomIndex]);
            }

            return sb.ToString();
        }
    }
}