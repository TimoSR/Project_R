using AppServices;
using Domain.DomainModels;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers.REST
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto newUserDto)
        {
            var newUser = new User
            {
                Email = newUserDto.Email,
                Password = newUserDto.Password
            };

            var result = await _authService.RegisterAsync(newUser);

            if (result)
            {
                return Ok(new { Message = "User successfully registered" });
            }

            return BadRequest(new { Message = "Email already exists" });
        }

        /// <summary>
        /// Authenticate a user
        /// </summary>
        [HttpPost("authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Authenticate([FromBody] AuthRequestDto requestDto)
        {
            var token = await _authService.AuthenticateAsync(requestDto.Email, requestDto.Password);

            if (token != null)
            {
                return Ok(new AuthResponseDto
                {
                    Token = token,
                    // ... other properties
                });
            }

            return Unauthorized(new { Message = "Invalid email or password" });
        }

        /// <summary>
        /// Logout a user
        /// </summary>
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Logout([FromBody] LogoutRequestDto requestDto)
        {
            var result = await _authService.LogoutAsync(requestDto.UserId);

            if (result)
            {
                return Ok(new { Message = "Logged out successfully" });
            }

            return BadRequest(new { Message = "Failed to logout" });
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteRequestDto requestDto)
        {
            var result = await _authService.DeleteUserAsync(requestDto.UserId);

            if (result)
            {
                return Ok(new { Message = "User deleted successfully" });
            }

            return BadRequest(new { Message = "Failed to delete user" });
        }
    }

    public class UserRegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    
    public class AuthRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LogoutRequestDto
    {
        public string UserId { get; set; }
    }

    public class DeleteRequestDto
    {
        public string UserId { get; set; }
    }
    
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}
