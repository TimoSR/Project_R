using _CommonLibrary.Patterns.ResultPattern._Enums;
using Application.AppServices._Interfaces;
using Application.DTO.Auth;
using Application.DTO.UserManagement;
using Infrastructure.Swagger;
using Infrastructure.Swagger.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers.REST.V2;

[ApiController]
[Route("api/v2/[controller]")]
[SwaggerDoc("Authentication")]
[ApiVersion("2.0")]
[Authorize]
public class AuthController : ControllerBase
{
    private readonly IAuthAppServiceV2 _authAppService;

    public AuthController(IAuthAppServiceV2 authAppService)
    {
        _authAppService = authAppService;
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
        var refreshedTokens = await _authAppService.RefreshTokenAsync(authRequestDto.RefreshToken);

        if (refreshedTokens != null)
        {
            return Ok(new 
            { 
                Message = "Token refreshed",
                refreshedTokens.Value.NewToken,
                refreshedTokens.Value.NewRefreshToken 
            });
        }

        return Unauthorized(new { Message = "Invalid token or unauthorized" });
    }

    // /// <summary>
    // /// Register a new user
    // /// </summary>
    // [AllowAnonymous]
    // [HttpPost("register")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // public async Task<IActionResult> Register([FromBody] UserRegisterDto newUserDto)
    // {
    //     if (!ModelState.IsValid)
    //     {
    //         return BadRequest(ModelState);
    //     }
    //     
    //     var newUser = new User
    //     {
    //         Email = newUserDto.Email,
    //         Password = newUserDto.Password
    //     };
    //
    //     var result = await _authService.RegisterAsync(newUser);
    //
    //     return result switch
    //     {
    //         UserRegistrationResult.Successful => Ok(new { Message = "User successfully registered" }),
    //         UserRegistrationResult.InvalidEmail => BadRequest(new { Message = "Invalid email address" }),
    //         UserRegistrationResult.EmailAlreadyExists => BadRequest(new { Message = "Email already exists" }),
    //         UserRegistrationResult.InvalidPassword => BadRequest(new { Message = "Password must have a minimum length of 6 and include at least one uppercase letter, number, and special symbol (e.g., !@#$%^&*)." }),
    //         _ => BadRequest(new { Message = "An unknown error occurred" })
    //     };
    // }

    /// <summary>
    /// Authenticate a user
    /// </summary>
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto requestDto)
    {
        var result = await _authAppService.LoginAsync(requestDto.Email, requestDto.Password);

        if (result.IsSuccess)
        {
            return Ok(new LoginResponseDto
            {
                AccessToken = result.Data.Token,
                RefreshToken = result.Data.RefreshToken
            });
        }

        return result.ErrorType switch
        {
            ServiceErrorType.BadRequest => BadRequest(new { result.Messages }),
            ServiceErrorType.Unauthorized => Unauthorized(new { result.Messages })
        };
    }

    /// <summary>
    /// Logout a user
    /// </summary>
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Logout([FromBody] LogoutRequestDto requestDto)
    {
        var result = await _authAppService.LogoutAsync(requestDto.UserId);

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
        var result = await _authAppService.DeleteUserAsync(requestDto.UserId);

        if (result)
        {
            return Ok(new { Message = "User deleted successfully" });
        }

        return BadRequest(new { Message = "Failed to delete user" });
    }
}

