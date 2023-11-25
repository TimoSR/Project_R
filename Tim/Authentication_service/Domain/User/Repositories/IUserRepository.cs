namespace Domain.User.Repositories
{
    public interface IUserRepository
    {
        Task<bool> RegisterUserIfNotRegisteredAsync(Entities.User newUser);
        Task<Entities.User> FindByEmailAsync(string email);
        Task<bool> IsUserAuthorized(string id);
        Task<string> ValidateRefreshTokenAsync(string refreshToken);
        Task CreateUserAsync(Entities.User user);
        Task UpdateRefreshTokenAsync(string userId, string refreshToken);
        Task<Entities.User> GetUserByRefreshTokenAsync(string refreshToken);
        Task<bool> DeleteUserAsync(string id);
    }
} 