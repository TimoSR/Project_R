using _CommonLibrary.Patterns.RegistrationHooks.Services._Interfaces;

namespace Application.AppServices._Interfaces;

public interface IAuthAppServiceV2 : IAppService
{
    Task<(string NewToken, string NewRefreshToken)?> RefreshTokenAsync(string refreshToken);
    Task<(string Token, string RefreshToken)?> LoginAsync(string email, string password);
    //Task<UserRegistrationResult> RegisterAsync(User newUser);
    Task<bool> LogoutAsync(string userId);
    Task<bool> DeleteUserAsync(string userId);
}