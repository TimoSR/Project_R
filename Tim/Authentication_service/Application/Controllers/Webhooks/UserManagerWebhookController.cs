using _CommonLibrary.Patterns.RegistrationHooks.Events._Attributes;
using Application.AppServices._Interfaces;
using Infrastructure.Swagger;
using Infrastructure.Swagger.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.UserManagement.Events;
using Infrastructure.Persistence._Interfaces;


namespace Application.Controllers.Webhooks;

[ApiController]
[Route("api/[controller]")]
[SwaggerDoc("UserManagerWebhook")]
[ApiVersion("1.0")]
[Authorize]
public class UserManagerWebhookController : ControllerBase
{
    private readonly IAuthAppServiceV1 _authAppService;
    private readonly IEventHandler _eventHandler;
    private readonly ILogger<UserManagerWebhookController> _logger;
    
    public UserManagerWebhookController(
        IAuthAppServiceV1 authAppService,
        IEventHandler eventHandler,
        ILogger<UserManagerWebhookController> logger)
    {
        _authAppService = authAppService;
        _eventHandler = eventHandler;
        _logger = logger;
    }
    
    // POST api/products
    [AllowAnonymous]
    [HttpPost("HandleUserCreatedEvent")]
    [EventSubscription("Auth-service-UserCreatedTopic")]
    public async Task<IActionResult> HandleUserCreatedEvent()
    {
        using StreamReader reader = new StreamReader(Request.Body);
        var body = await reader.ReadToEndAsync();
        _eventHandler.ProcessReceivedEvent<UserCreatedEvent>(body);
        return Ok();
    }
}