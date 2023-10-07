using Domain.DomainModels;

namespace Domain.Common.IRepositories
{
    public interface IUserRepository
    {
        Task<User> FindByEmailAsync(string email);
        Task<bool> IsUserAuthorized(string id);
        Task<string> ValidateRefreshTokenAsync(string refreshToken);
        Task CreateUserAsync(User user);
        Task UpdateRefreshTokenAsync(string userId, string refreshToken);
        Task<User> GetUserByRefreshTokenAsync(string refreshToken);
        Task<bool> DeleteUserAsync(string id);
    }
} 