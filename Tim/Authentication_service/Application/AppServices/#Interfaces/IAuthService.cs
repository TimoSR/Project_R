using Application.Registrations._Interfaces;
using Domain.DomainModels;
using Domain.DomainModels.Enums;

namespace Application.AppServices._Interfaces;

public interface IAuthService : IAppService
{
    Task<bool> AuthenticateAsync(string jwtToken);
    Task<string> RefreshTokenAsync(string refreshToken);
    Task<(string Token, string RefreshToken)?> LoginAsync(string email, string password);
    Task<UserRegistrationResult> RegisterAsync(User newUser);
    Task<bool> LogoutAsync(string userId);
    Task<bool> DeleteUserAsync(string userId);
}