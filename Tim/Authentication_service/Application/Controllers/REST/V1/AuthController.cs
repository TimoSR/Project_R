using _CommonLibrary.Patterns.ResultPattern;
using Application.DataTransferObjects.UserManagement;
using Application.DTO.Auth;
using Domain.User.Entities;
using Domain.User.Services;
using Infrastructure.Swagger;
using Infrastructure.Swagger.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers.REST.V1;

[ApiController]
[Route("api/v1/[controller]")]
[SwaggerDoc("Authentication")]
[ApiVersion("1.0")]
[Authorize]
public class AuthController : ControllerBase
{
    private readonly IAuthServiceV1 _authServiceV1;

    public AuthController(IAuthServiceV1 authServiceV1)
    {
        _authServiceV1 = authServiceV1;
    }
    
    /// <summary>
    /// Check if the user's token is valid
    /// </summary>
    [HttpGet("check-token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult CheckTokenValidity()
    {
        // Token Validation happens in the middleware
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
        var result = await _authServiceV1.RefreshTokenAsync(authRequestDto.RefreshToken);

        if (result.IsSuccess)
        {
            return Ok(new 
            {
                AccessToken = result.Data.NewToken,
                RefreshToken = result.Data.NewRefreshToken 
            });
        }

        return result.ErrorType switch
        {
            ServiceErrorType.BadRequest => BadRequest(new { result.Message }),
            ServiceErrorType.Unauthorized => Unauthorized(new { result.Message }),
            _ => StatusCode(StatusCodes.Status500InternalServerError, new { result.Message })
        };
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
        var newUser = new User
        {
            Email = newUserDto.Email,
            Password = newUserDto.Password
        };

        var result = await _authServiceV1.RegisterAsync(newUser);

        if (result.IsSuccess)
        {
            return Ok(new { result.Message });
        }

        return result.ErrorType switch
        {
            ServiceErrorType.BadRequest => BadRequest(new { Message = result.Message }),
            ServiceErrorType.Unauthorized => Unauthorized(new { Message = result.Message }),
            _ => StatusCode(StatusCodes.Status500InternalServerError, new { Message = result.Message })
        };
    }

    /// <summary>
    /// Authenticate a user
    /// </summary>
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto requestDto)
    {
        var result = await _authServiceV1.LoginAsync(requestDto.Email, requestDto.Password);

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
            ServiceErrorType.BadRequest => BadRequest(new { result.Message }),
            ServiceErrorType.Unauthorized => Unauthorized(new { result.Message }),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
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
        var result = await _authServiceV1.LogoutAsync(requestDto.UserId);

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
        var result = await _authServiceV1.DeleteUserAsync(requestDto.UserId);

        if (result)
        {
            return Ok(new { Message = "User deleted successfully" });
        }

        return BadRequest(new { Message = "Failed to delete user" });
    }
}

