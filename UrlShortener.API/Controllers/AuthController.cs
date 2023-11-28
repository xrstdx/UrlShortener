using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Abstractions.Services;
using UrlShortener.Application.DTOs.Auth;

namespace UrlShortener.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationDTO model)
        {
            await _authService.RegisterAsync(model);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            string token = await _authService.LoginAsync(model);

            return Ok(token);
        }
    }
}
