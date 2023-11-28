using _SharedKernel.Patterns.RegistrationHooks.Events._Attributes;
using _SharedKernel.Patterns.ResultPattern._Enums;
using Application.AppServices._Interfaces;
using Application.Controllers.Webhooks._Abstract;
using Domain._Shared.Events.UserManagement;
using Infrastructure.Persistence._Interfaces;
using Infrastructure.Swagger;
using Infrastructure.Swagger.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers.Webhooks;

[ApiController]
[Route("api/[controller]")]
[SwaggerDoc("UserManagerWebhooks")]
[ApiVersion("1.0")]
[Authorize]
public class UserManagerWebhooks : BaseWebhookController
{
    private readonly IAuthAppServiceV1 _authAppService;
    
    public UserManagerWebhooks(
        IAuthAppServiceV1 authAppService,
        IEventHandler eventHandler)
        : base(eventHandler)
    {
        _authAppService = authAppService;
    }
    
    [AllowAnonymous]
    [HttpPost("HandleUserRegInitEvent")]
    [EventSubscription("Auth-service-UserRegInitTopic")]
    public async Task<IActionResult> HandleUserRegInitEvent()
    {
        var data = await OnEvent<UserRegInitEvent>();
        var result = await _authAppService.SetUserAuthDetailsAsync(data);

        if (result.IsSuccess)
        {
            return Ok(new { result.Messages });
        }

        return result.ErrorType switch
        {
            ServiceErrorType.BadRequest => BadRequest(new { Message = result.Messages }),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    [AllowAnonymous]
    [HttpPost("HandleUserDeletionInitEvent")]
    [EventSubscription("Auth-service-UserDeletionInitTopic")]
    public async Task<IActionResult> HandleUserDeletionInitEvent()
    {
        var data = await OnEvent<UserDeletionInitEvent>();
        var result = await _authAppService.SetUserAuthDetailsAsync(data);

        if (result.IsSuccess)
        {
            return Ok(new { result.Messages });
        }

        return result.ErrorType switch
        {
            ServiceErrorType.BadRequest => BadRequest(new { Message = result.Messages }),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}