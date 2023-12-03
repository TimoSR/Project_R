using _SharedKernel.Patterns.ResultPattern;
using Application.AppServices._Interfaces;
using Application.Controllers.GraphQL.GraphQL._Interfaces;
using Application.DTO.Auth;

namespace Application.Controllers.GraphQL.GraphQL;

public class AuthQueries : IQuery
{
    
    private readonly IAuthAppServiceV1 _authAppService;
    
    public AuthQueries(IAuthAppServiceV1 authAppService)
    {
        _authAppService = authAppService;
    }

    public async Task<ServiceResult> DoNotUse(LoginRequestDto requestDto)
    {
        var result = await _authAppService.LoginAsync(requestDto.Email, requestDto.Password);
            
        return !result.IsSuccess ? result : result;
    }
}