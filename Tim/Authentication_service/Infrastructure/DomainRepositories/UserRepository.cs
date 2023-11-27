using Domain.UserManagement.Entities;
using Domain.UserManagement.Repositories;
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

        public Task<IEnumerable<User>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        private async Task<bool> CheckUserExistsAsync(string email)
        {
            var collection = GetCollection();
            var existingUser = await collection.Find(u => u.Email == email).FirstOrDefaultAsync();
            return existingUser != null;
        }

        public async Task<bool> CreateUserIfNotRegisteredAsync(User newUser)
        {
            using var session = await _dbManager.GetClient().StartSessionAsync();
            session.StartTransaction();
            try
            {
                // Use CheckUserExistsAsync to check if the user already exists
                if (await CheckUserExistsAsync(newUser.Email))
                {
                    // User already exists, no need to proceed further. Rollback any changes.
                    await session.AbortTransactionAsync();
                    return false;
                }

                var collection = GetCollection();
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

        public Task UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserEmailAsync(string userId, string newEmail)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserPasswordAsync(string userId, string newPassword)
        {
            throw new NotImplementedException();
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

        public async Task<bool> DeleteUserAsync(string id)
        {
            return await DeleteAsync(id);
        }
    }
}