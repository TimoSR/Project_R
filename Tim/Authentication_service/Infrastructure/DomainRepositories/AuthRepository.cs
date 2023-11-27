using Domain.UserAuthentication.Repositories;
using Domain.UserManagement.Entities;
using Infrastructure.Persistence._Interfaces;
using Infrastructure.Persistence.MongoDB;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Infrastructure.DomainRepositories;

public class AuthRepository : MongoRepository<User>, IAuthRepository
{
    public AuthRepository(IMongoDbManager dbManager, ILogger<AuthRepository> logger) : base(dbManager, logger)
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
            _logger.LogError($"Error retrieving user by email {email}: {ex.Message}");
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
}