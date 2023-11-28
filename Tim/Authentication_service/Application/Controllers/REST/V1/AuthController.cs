using _SharedKernel.Patterns.ResultPattern._Enums;
using Application.AppServices._Interfaces;
using Application.DTO.Auth;
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
    private readonly IAuthAppServiceV1 _authAppService;

    public AuthController(IAuthAppServiceV1 authAppService)
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
    public async Task<IActionResult> RefreshToken(AuthRequestDto authRequestDto)
    {
        var result = await _authAppService.RefreshTokenAsync(authRequestDto.RefreshToken);

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
            ServiceErrorType.BadRequest => BadRequest(new { result.Messages }),
            ServiceErrorType.Unauthorized => Unauthorized(new { result.Messages })
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
    public async Task<IActionResult> Login(LoginRequestDto requestDto)
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
    public async Task<IActionResult> Logout(string userId)
    {
        var result = await _authAppService.LogoutAsync(userId);

        if (result)
        {
            return Ok(new { Message = "Logged out successfully" });
        }

        return BadRequest(new { Message = "Failed to logout" });
    }
}

