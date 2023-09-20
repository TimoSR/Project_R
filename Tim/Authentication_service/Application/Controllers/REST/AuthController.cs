using System;
using System.Threading.Tasks;
using Application.AppServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(AuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("authenticate-google")]
        public async Task<IActionResult> AuthenticateGoogleUserAsync([FromBody] string googleToken)
        {
            try
            {
                var (user, accessToken, refreshToken) = await _authService.AuthenticateGoogleUserAsync(googleToken);

                if (user == null)
                {
                    return Unauthorized("Invalid Google token.");
                }

                return Ok(new
                {
                    user,
                    accessToken,
                    refreshToken
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(AuthenticateGoogleUserAsync)}: {ex.Message}");
                return BadRequest("An error occurred while authenticating.");
            }
        }
    }
}