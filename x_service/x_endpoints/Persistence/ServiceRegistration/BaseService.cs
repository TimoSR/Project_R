using MongoDB.Driver;
using x_endpoints.Persistence.MongoDB;

namespace x_endpoints.Persistence.ServiceRegistration;

public abstract class BaseService<T>
{
        protected readonly IMongoCollection<T> _collection;

        protected BaseService(MongoDbService dbService, string collectionName)
        {
            _collection = dbService.GetDefaultDatabase().GetCollection<T>(collectionName);
        }

        public virtual async Task InsertAsync(T data)
        {
            await _collection.InsertOneAsync(data);
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public virtual async Task UpdateAsync(string id, T updatedData)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            UpdateDefinition<T> updateDefinition = CreateUpdateDefinition(updatedData);
            
            if (updateDefinition != null)
            {
                await _collection.UpdateOneAsync(filter, updateDefinition);
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
            var result = await _collection.DeleteOneAsync(filter);

            return result.IsAcknowledged && result.DeletedCount > 0;
        }
}