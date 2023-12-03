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

    public async Task<ServiceResult<LoginResponseDto>> LoginAsync(LoginRequestDto requestDto)
    {
        var result = await _authAppService.LoginAsync(requestDto.Email, requestDto.Password);

        if (result.IsSuccess)
        {
            return ServiceResult<LoginResponseDto>.Success(new LoginResponseDto
            {
                AccessToken = result.Data.Token,
                RefreshToken = result.Data.RefreshToken
            });
        }

        // Convert nullable strings to non-nullable and filter out any null values.
        var nonNullableMessages = result.Messages.Where(m => m != null).Select(m => m!);

        return ServiceResult<LoginResponseDto?>.Failure(nonNullableMessages, result.ErrorType);
    }
}