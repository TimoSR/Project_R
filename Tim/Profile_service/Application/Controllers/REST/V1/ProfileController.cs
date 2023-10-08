using Application.AppServices.V1._Interfaces;
using Infrastructure.Swagger;
using Infrastructure.Swagger.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers.REST.V1;

[ApiController]
[Route("api/v1/[controller]")]
[SwaggerDoc("Profile")]
[ApiVersion("1.0")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly ILogger<ProfileController> _logger;
    private readonly IProfileServiceV1 _profileService;

    public ProfileController(
        ILogger<ProfileController> logger,
        IProfileServiceV1 profileService)
    {
        _logger = logger;
        _profileService = profileService;
    }
    
    
}