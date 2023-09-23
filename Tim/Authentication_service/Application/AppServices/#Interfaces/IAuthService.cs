using Application.Registrations._Interfaces;
using Domain.DomainModels;
using Domain.DomainModels.Enums;

namespace Application.AppServices._Interfaces;

public interface IAuthService : IAppService
{
    Task<string> AuthenticateAsync(string email, string password);
    Task<UserRegistrationResult> RegisterAsync(User newUser);
    Task<bool> LogoutAsync(string userId);
    Task<bool> DeleteUserAsync(string userId);
}