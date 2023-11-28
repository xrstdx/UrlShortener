using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Abstractions.Services;

namespace UrlShortener.API.Controllers
{
    [ApiController]
    [Route("r")]
    public class RedirectController : ControllerBase
    {
        private readonly IUrlRecordsService _urlRecordsService;

        public RedirectController(IUrlRecordsService urlRecordsService)
        {
            _urlRecordsService = urlRecordsService;
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> RedirectToOriginalUrl(string code)
        {
            var originalUrl = await _urlRecordsService.GetOriginalUrlByCodeAsync(code);

            return Redirect(originalUrl);
        }
    }
}
