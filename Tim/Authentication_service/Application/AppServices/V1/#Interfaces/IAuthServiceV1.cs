using _CommonLibrary.Patterns._Interfaces;
using Application.Registrations._Interfaces;
using Domain.DomainModels;
using Domain.DomainModels.Enums;

namespace Application.AppServices.V1._Interfaces;

public interface IAuthServiceV1 : IAppService
{
    Task<IServiceResult> RefreshTokenAsync(string refreshToken);
    Task<IServiceResult> LoginAsync(string email, string password);
    Task<IServiceResult> RegisterAsync(User newUser);
    Task<bool> LogoutAsync(string userId);
    Task<bool> DeleteUserAsync(string userId);
}