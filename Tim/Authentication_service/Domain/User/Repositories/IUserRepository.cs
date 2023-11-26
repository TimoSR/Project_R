namespace Domain.User.Repositories;

public interface IUserRepository
{
    Task<Entities.User> GetUserByIdAsync(string id);
    Task<bool> CreateUserIfNotRegisteredAsync(Entities.User newUser);
    Task<Entities.User> FindByEmailAsync(string email);
    Task<bool> IsUserAuthorized(string id);
    Task<string> ValidateRefreshTokenAsync(string refreshToken);
    Task UpdateRefreshTokenAsync(string userId, string refreshToken);
    Task<Entities.User> GetUserByRefreshTokenAsync(string refreshToken);
    Task<bool> DeleteUserAsync(string id);
}