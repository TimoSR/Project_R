using MongoDB.Driver;
using x_endpoints.Models;
using System.Collections.Generic;
using x_endpoints.Persistence.MongoDB; 

public abstract class BaseService<T> 
{ 
    private readonly IMongoCollection<T> _collection;

    protected BaseService(MongoDbService dbService, string collectionName)
    {
        _collection = dbService.GetDefaultDatabase().GetCollection<T>(collectionName);
    }

    public async Task InsertData(T data)
    {
        await _collection.InsertOneAsync(data);
    }

    public async Task<List<T>> GetAllAsync()
    {
        var dataList = await _collection.Find(_ => true).ToListAsync();
        return dataList;
    }

    public async Task<T> GetByIdAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq("Id", id);
        var data = await _collection.Find(filter).FirstOrDefaultAsync();
        return data;
    }

    public async Task UpdateAsync(string id, T updatedData)
    {
        var filter = Builders<T>.Filter.Eq("Id", id);
        
        var updateProps = typeof(T).GetProperties();
        var updateDefinitionBuilder = Builders<T>.Update;

        UpdateDefinition<T> updateDefinition = null;

        foreach (var prop in updateProps)
        {
            var propValue = prop.GetValue(updatedData);
            var update = updateDefinitionBuilder.Set(prop.Name, propValue);
            
            if (updateDefinition == null)
            {
                updateDefinition = update;
            }
            else
            {
                updateDefinition = Builders<T>.Update.Combine(updateDefinition, update);
            }
        }

        if (updateDefinition != null)
        {
            await _collection.UpdateOneAsync(filter, updateDefinition);
        }
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq("Id", id);
        var result = await _collection.DeleteOneAsync(filter);

        return result.IsAcknowledged && result.DeletedCount > 0;
    } 
}