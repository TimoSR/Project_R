using Domain.User.Entities;
using Domain.User.Repositories;
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

        public Task<User> GetUserByIdAsync(string id)
        {
            return GetByIdAsync(id);
        }

        public async Task<bool> CreateUserIfNotRegisteredAsync(User newUser)
        {
            using var session = await _dbManager.GetClient().StartSessionAsync();
            session.StartTransaction();
            try
            {
                var collection = GetCollection();
                var existingUser = await collection.Find(session, u => u.Email == newUser.Email).FirstOrDefaultAsync();
                if (existingUser != null)
                {
                    // User already exists, no need to proceed further. Rollback any changes.
                    await session.AbortTransactionAsync();
                    return false;
                }

                await collection.InsertOneAsync(session, newUser);
                // Commit the transaction. If an error occurs during commit, it will throw an exception.
                await session.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                // If any exception occurs during the transaction, rollback changes.
                await session.AbortTransactionAsync();
                _logger.LogError($"Error during user registration: {ex.Message}");
                throw;
            }
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

        public async Task<bool> IsUserAuthorized(string id)
        {
            try
            {
                var collection = GetCollection();
                var user = await collection.Find(u => u.Id == id).FirstOrDefaultAsync();
        
                if (user == null)
                {
                    _logger.LogWarning($"User with ID {id} not found.");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error checking if user with ID {id} is authorized: {ex.Message}");
                throw;
            }
        }

        public async Task<string> ValidateRefreshTokenAsync(string refreshToken)
        {
            try
            {
                var collection = GetCollection();
                
                // Find the user with the matching refresh token
                var user = await collection
                    .Find(u => u.RefreshToken == refreshToken)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    _logger.LogWarning($"No user found with the provided refresh token.");
                    return null;
                }

                return user.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error validating refresh token: {ex.Message}");
                throw;
            }
        }
        
        public async Task UpdateRefreshTokenAsync(string userId, string refreshToken)
        {
            try
            {
                var collection = GetCollection();
                var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
                var update = Builders<User>.Update
                    .Set(u => u.RefreshToken, refreshToken);
                
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