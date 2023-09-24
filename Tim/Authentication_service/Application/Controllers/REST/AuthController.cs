using Application.AppServices;
using Application.AppServices._Interfaces;
using Application.DataTransferObjects.Auth;
using Application.DataTransferObjects.UserManagement;
using Domain.DomainModels;
using Domain.DomainModels.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers.REST
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
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

            return result switch
            {
                UserRegistrationResult.Successful => Ok(new { Message = "User successfully registered" }),
                UserRegistrationResult.InvalidEmail => BadRequest(new { Message = "Invalid email address" }),
                UserRegistrationResult.EmailAlreadyExists => BadRequest(new { Message = "Email already exists" }),
                UserRegistrationResult.InvalidPassword => BadRequest(new { Message = "Password must have a minimum length of 6 and include at least one uppercase letter, number, and special symbol (e.g., !@#$%^&*)." }),
                _ => BadRequest(new { Message = "An unknown error occurred" })
            };
        }

        /// <summary>
        /// Authenticate a user
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] AuthRequestDto requestDto)
        {
            var token = await _authService.LoginAsync(requestDto.Email, requestDto.Password);

            if (token != null)
            {
                return Ok(new AuthResponseDto
                {
                    Token = token,
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
}
