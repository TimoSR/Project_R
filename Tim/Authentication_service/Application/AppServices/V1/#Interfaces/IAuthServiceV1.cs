using _CommonLibrary.Patterns._Interfaces;
using _CommonLibrary.Patterns.ResultPattern;
using Application.Registrations._Interfaces;
using Domain.DomainModels;

namespace Application.AppServices.V1._Interfaces;

public interface IAuthServiceV1 : IAppService
{
    Task<ServiceResult<(string NewToken, string NewRefreshToken)>?> RefreshTokenAsync(string refreshToken);
    Task<ServiceResult<(string Token, string RefreshToken)>?> LoginAsync(string email, string password);
    Task<IServiceResult> RegisterAsync(User newUser);
    Task<bool> LogoutAsync(string userId);
    Task<bool> DeleteUserAsync(string userId);
}