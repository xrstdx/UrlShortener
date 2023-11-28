using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrlShortener.Application.Abstractions.Services;
using UrlShortener.Application.DTOs.UrlRecords;


namespace UrlShortener.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/urlrecords")]
    public class UrlRecordsController : ControllerBase
    {
        private readonly IUrlRecordsService _urlRecordsService;

        public UrlRecordsController(IUrlRecordsService urlRecordsService)
        {
            _urlRecordsService = urlRecordsService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUrlRecordDTO model)
        {
            var user = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            await _urlRecordsService.CreateAsync(user?.Value, model);

            return Ok();
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var urlRecord = await _urlRecordsService.GetByIdAsync(id);

            return Ok(urlRecord);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int amount = 20, [FromQuery] int page = 0)
        {
            var urlRecords = await _urlRecordsService.GetAllAsync(amount, page);

            return Ok(urlRecords);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            var user = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            await _urlRecordsService.DeleteByIdAsync(user?.Value, id);

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            await _urlRecordsService.DeleteAllAsync();

            return Ok();
        }
    }
}
