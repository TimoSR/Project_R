using Domain.UserAuthentication.Entities;

namespace Domain.UserAuthentication.Repositories;

public interface IAuthRepository
{
    // Create Operations
    Task SetUserDetails(string userId, string email, string password);
    
    // Read Operations
    Task<AuthUser> FindByEmailAsync(string email);
    Task<bool> IsUserAuthorized(string id);
    Task<string> ValidateRefreshTokenAsync(string refreshToken);
    Task<AuthUser> GetUserByRefreshTokenAsync(string refreshToken);
    
    // Update Operation
    Task UpdateRefreshTokenAsync(string userId, string refreshToken);
}