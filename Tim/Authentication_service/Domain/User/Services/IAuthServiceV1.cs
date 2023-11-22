using _CommonLibrary.Patterns.ResultPattern;
using Domain._SharedKernel.Services._Interfaces;

namespace Domain.User.Services;

public interface IAuthServiceV1 : IService
{
    Task<ServiceResult<(string NewToken, string NewRefreshToken)>?> RefreshTokenAsync(string refreshToken);
    Task<ServiceResult<(string Token, string RefreshToken)>?> LoginAsync(string email, string password);
    Task<ServiceResult> RegisterAsync(Entities.User newUser);
    Task<bool> LogoutAsync(string userId);
    Task<bool> DeleteUserAsync(string userId);
}