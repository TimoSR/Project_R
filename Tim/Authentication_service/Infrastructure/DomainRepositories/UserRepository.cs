using System.Threading.Tasks;
using Domain.DomainModels;
using Domain.IRepositories;
using Infrastructure.Persistence._Interfaces;
using Infrastructure.Persistence.MongoDB;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Infrastructure.DomainRepositories
{
    public class UserRepository : MongoRepository<User>, IUserRepository
    {
        public UserRepository(IMongoDbManager dbManager, ILogger<UserRepository> logger)
            : base(dbManager, logger)
        {
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            try
            {
                var collection = GetCollection();
                return await collection.Find(u => u.Email == email).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                var collectionName = nameof(User) + "s";
                _logger.LogError($"Error retrieving user by email {email} from {collectionName}: {ex.Message}");
                throw;
            }
        }

        public async Task CreateUserAsync(User user)
        {
            await InsertAsync(user);
        }
        
        public async Task UpdateRefreshTokenAsync(string userId, string refreshToken, DateTime expiryTime)
        {
            try
            {
                var collection = GetCollection();
                var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
                var update = Builders<User>.Update
                    .Set(u => u.RefreshToken, refreshToken)
                    .Set(u => u.RefreshTokenExpiryTime, expiryTime);
                
                await collection.UpdateOneAsync(filter, update);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating refresh token for user ID {userId}: {ex.Message}");
                throw;
            }
        }

        public async Task<User> GetUserByRefreshTokenAsync(string refreshToken)
        {
            try
            {
                var collection = GetCollection();
                return await collection.Find(u => u.RefreshToken == refreshToken).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving user by refresh token: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            return await DeleteAsync(id);
        }
    }
}