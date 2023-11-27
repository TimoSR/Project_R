using Domain.UserAuthentication.Entities;
using Domain.UserManagement.Entities;

namespace Domain.UserAuthentication.Repositories;

public interface IAuthRepository
{
    // Read Operations
    Task<User> FindByEmailAsync(string email);
    Task<bool> IsUserAuthorized(string id);
    Task<string> ValidateRefreshTokenAsync(string refreshToken);
    Task<User> GetUserByRefreshTokenAsync(string refreshToken);

    // Update Operation
    Task UpdateRefreshTokenAsync(string userId, string refreshToken);
}