using Domain.DomainModels;

namespace Domain.IRepositories
{
    public interface IUserRepository
    {
        Task<User> FindByEmailAsync(string email);
        Task CreateUserAsync(User user);

        Task UpdateRefreshTokenAsync(string userId, string refreshToken, DateTime expiryTime);
    }
}