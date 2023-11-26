using _CommonLibrary.Patterns.ResultPattern._Enums;
using Application.AppServices._Interfaces;
using Application.DTO.UserManagement;
using Infrastructure.Swagger;
using Infrastructure.Swagger.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers.REST.V1;

[ApiController]
[Route("api/v1/[controller]")]
[SwaggerDoc("User")]
[ApiVersion("1.0")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(UserRegisterDto newUserDto)
    {
        var result = await _userService.RegisterAsync(newUserDto);

        if (result.IsSuccess)
        {
            return Ok(new { result.Messages });
        }

        return result.ErrorType switch
        {
            ServiceErrorType.BadRequest => BadRequest(new { Message = result.Messages })
        };
    }
    
    /// <summary>
    /// Register a new user
    /// </summary>
    [HttpGet("getById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _userService.GetUserByIdAsync(id);

        if (result.IsSuccess)
        {
            return Ok(new { result.Data });
        }   
        
        return BadRequest(new { Message = result.Messages});
    }

    /// <summary>
    /// Delete a user
    /// </summary>
    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        var result = await _userService.DeleteUserAsync(userId);

        if (result)
        {
            return Ok(new { Message = "User deleted successfully" });
        }

        return BadRequest(new { Message = "Failed to delete user" });
    }
}