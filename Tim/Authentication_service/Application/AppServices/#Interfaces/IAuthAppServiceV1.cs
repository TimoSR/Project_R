using _SharedKernel.Patterns.RegistrationHooks.Services._Interfaces;
using _SharedKernel.Patterns.ResultPattern;

namespace Application.AppServices._Interfaces;

public interface IAuthAppServiceV1 : IAppService
{
    Task<ServiceResult<(string NewToken, string NewRefreshToken)>?> RefreshTokenAsync(string refreshToken);
    Task<ServiceResult<(string Token, string RefreshToken)>?> LoginAsync(string email, string password);
    Task<bool> LogoutAsync(string userId);
}