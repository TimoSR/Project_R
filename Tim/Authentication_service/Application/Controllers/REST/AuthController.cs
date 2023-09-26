using Application.AppServices._Interfaces;
using Application.DataTransferObjects.Auth;
using Application.DataTransferObjects.UserManagement;
using Domain.DomainModels;
using Domain.DomainModels.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers.REST
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        
        /// <summary>
        /// Check if the user's token is valid
        /// </summary>
        [HttpGet("check-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult CheckTokenValidity()
        {
            // If this point is reached, the token is valid
            return Ok(new { Message = "Token is valid" });
        }
        
        /// <summary>
        /// Refresh a user's token
        /// </summary>
        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken([FromBody] AuthRequestDto authRequestDto)
        {
            // Since JwtMiddleware takes care of authentication, you can trust that the request is authenticated at this point.
        
            // Now you can focus on the token refresh logic.
            var newToken = await _authService.RefreshTokenAsync(authRequestDto.RefreshToken);

            if (newToken != null)
            {
                return Ok(new 
                { 
                    Message = "Token refreshed",
                    NewToken = newToken 
                });
            }

            return Unauthorized(new { Message = "Invalid token or unauthorized" });
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto newUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
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
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto requestDto)
        {
            var result = await _authService.LoginAsync(requestDto.Email, requestDto.Password);

            if (result != null)
            {
                var (token, refreshToken) = result.Value;  // Use Value to get the underlying non-nullable tuple
                
                return Ok(new LoginResponseDto
                {
                    AccessToken = token,
                    RefreshToken = refreshToken  // Assuming you've added this to LoginResponseDto
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
