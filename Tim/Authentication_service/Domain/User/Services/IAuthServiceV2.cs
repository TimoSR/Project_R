using Domain._SharedKernel.Services._Interfaces;

namespace Domain.User.Services;

public interface IAuthServiceV2 : IService
{
    Task<(string NewToken, string NewRefreshToken)?> RefreshTokenAsync(string refreshToken);
    Task<(string Token, string RefreshToken)?> LoginAsync(string email, string password);
    //Task<UserRegistrationResult> RegisterAsync(User newUser);
    Task<bool> LogoutAsync(string userId);
    Task<bool> DeleteUserAsync(string userId);
}