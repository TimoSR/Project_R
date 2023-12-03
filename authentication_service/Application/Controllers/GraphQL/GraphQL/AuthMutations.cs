using _SharedKernel.Patterns.ResultPattern;
using Application.AppServices._Interfaces;
using Application.Controllers.GraphQL.GraphQL._Interfaces;
using Application.DTO.Auth;

namespace Application.Controllers.GraphQL.GraphQL;

public class AuthMutations : IMutation
{
    private readonly IAuthAppServiceV1 _authAppService;

    public AuthMutations(IAuthAppServiceV1 authAppService)
    {
        _authAppService = authAppService;
    }

    public async Task<ServiceResult> LoginAsync(LoginRequestDto requestDto)
    {
        var result = await _authAppService.LoginAsync(requestDto.Email, requestDto.Password);
            
        return !result.IsSuccess ? result : result;
    }
}