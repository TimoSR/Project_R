namespace Domain.User.Repositories;

public interface IAuthRepository
{
    // Read Operations
    Task<bool> IsUserAuthorized(string id);
    Task<string> ValidateRefreshTokenAsync(string refreshToken);
    Task<Entities.User> GetUserByRefreshTokenAsync(string refreshToken);

    // Update Operation
    Task UpdateRefreshTokenAsync(string userId, string refreshToken);
}