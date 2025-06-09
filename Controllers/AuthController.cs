using ArtMarketPlaceAPI.DTO;
using ArtMarketPlaceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArtMarketPlaceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register registerDto)
        {
            var result = await _authService.RegisterUserAsync(registerDto);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);
            return Ok(result);
        }
        
    }
    
}
