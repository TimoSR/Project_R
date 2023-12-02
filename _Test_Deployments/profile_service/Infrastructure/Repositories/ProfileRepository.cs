using Domain.Profile.Entities;
using Domain.Profile.Repository;
using Infrastructure.Persistence._Interfaces;
using Infrastructure.Persistence.MongoDB;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class ProfileRepository : MongoRepository<Profile>, IProfileRepository
{
    public ProfileRepository(IMongoDbManager dbManager, ILogger<ProfileRepository> logger) : base(dbManager, logger)
    {
    }
    
    public async Task CreateProfileAsync(Profile profile)
    {
        try
        {
            await InsertAsync(profile);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating profile: {ex.Message}");
            throw;
        }
    }

    public async Task<Profile> GetProfileInfoAsync(string userId)
    {
        try
        {
            var collection = GetCollection();
            return await collection.Find(p => p.UserId == userId).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving profile by ID {userId}: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> UpdateProfileAsync(Profile profile)
    {
        try
        {
            var collection = GetCollection();
            var filter = Builders<Profile>.Filter.Eq(p => p.UserId, profile.UserId);
            var result = await collection.ReplaceOneAsync(filter, profile);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating profile: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> DeleteProfileAsync(string userId)
    {
        try
        {
            var filter = Builders<Profile>.Filter.Eq("UserId", userId);
            return await DeleteFilterAsync(filter);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting profile with UserId {userId}: {ex.Message}");
            throw;
        }
    }
    
}