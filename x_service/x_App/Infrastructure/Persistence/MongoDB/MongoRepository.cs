using MongoDB.Driver;
using x_endpoints.DomainRepositories._Interfaces;
using x_endpoints.Infrastructure.Persistence._Interfaces;

namespace x_endpoints.Infrastructure.Persistence.MongoDB;

public abstract class MongoRepository<T> : IRepository<T>
{
    protected abstract string CollectionName { get; }

    private readonly IMongoDbManager _dbManager;

    protected MongoRepository(IMongoDbManager dbManager)
    {
        _dbManager = dbManager;
    }

    protected IMongoCollection<T> GetCollection() => _dbManager.GetCollection<T>(CollectionName);

    public virtual async Task InsertAsync(T data)
    {
        var collection = GetCollection();
        await collection.InsertOneAsync(data);
    }

    public virtual async Task<List<T>> GetAllAsync()
    {
        var collection = GetCollection();
        return await collection.Find(_ => true).ToListAsync();
    }

    public virtual async Task<T> GetByIdAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq("Id", id);
        var collection = GetCollection();
        return await collection.Find(filter).FirstOrDefaultAsync();
    }

    public virtual async Task UpdateAsync(string id, T updatedData)
    {
        var filter = Builders<T>.Filter.Eq("Id", id);
        UpdateDefinition<T> updateDefinition = CreateUpdateDefinition(updatedData);
        var collection = GetCollection();
        
        if (updateDefinition != null)
        {
            await collection.UpdateOneAsync(filter, updateDefinition);
        }
    }

    private UpdateDefinition<T> CreateUpdateDefinition(T updatedData)
    {
        var updateProps = typeof(T).GetProperties();
        var updateDefinitionBuilder = Builders<T>.Update;
        UpdateDefinition<T> updateDefinition = null;

        foreach (var prop in updateProps)
        {
            var propValue = prop.GetValue(updatedData);
            var update = updateDefinitionBuilder.Set(prop.Name, propValue);
            
            updateDefinition = updateDefinition == null 
                               ? update 
                               : Builders<T>.Update.Combine(updateDefinition, update);
        }

        return updateDefinition;
    }

    public virtual async Task<bool> DeleteAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq("Id", id);
        var collection = GetCollection();
        var result = await collection.DeleteOneAsync(filter);

        return result.IsAcknowledged && result.DeletedCount > 0;
    }
}