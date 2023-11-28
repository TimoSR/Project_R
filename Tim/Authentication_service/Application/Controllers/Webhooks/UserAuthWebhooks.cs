using Application.AppServices._Interfaces;
using Application.Controllers.Webhooks._Abstract;
using Infrastructure.Persistence._Interfaces;
using Infrastructure.Swagger;
using Infrastructure.Swagger.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers.Webhooks;

[ApiController]
[Route("api/")]
[SwaggerDoc("UserAuthWebhooks")]
[ApiVersion("1.0")]
[Authorize]
public class UserAuthWebhooks : BaseWebhookController
{
    
    private readonly IUserService _userService;
    
    public UserAuthWebhooks(
        IUserService userService,
        IEventHandler eventHandler
        ) : base(eventHandler)
    {
        _userService = userService;
    }
}