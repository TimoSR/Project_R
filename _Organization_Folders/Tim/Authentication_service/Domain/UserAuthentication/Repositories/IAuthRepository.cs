using Domain.UserAuthentication.Entities;

namespace Domain.UserAuthentication.Repositories;

public interface IAuthRepository
{
    // Create Operations
    Task<bool> SetUserAuthDetails(AuthUser userDetails);
    
    // Read Operations
    Task<AuthUser> FindByEmailAsync(string email);
    Task<bool> IsUserAuthorized(string id);
    Task<string> ValidateRefreshTokenAsync(string refreshToken);
    Task<AuthUser> GetUserByRefreshTokenAsync(string refreshToken);

    // Update Operations
    Task UpdateRefreshTokenAsync(string userId, string refreshToken);
    
    // Delete Operations
    Task<bool> DeleteUserByEmailAsync(string email);
}