using MongoDB.Driver;
using x_endpoints.Persistence._Interfaces;

namespace x_endpoints.DomainServices;

public abstract class BaseService<T>
{
        protected readonly IMongoCollection<T> Collection;

        protected BaseService(
            IMongoDbManager mongoDbManager, 
            string collectionName
            )
        { 
            Collection = mongoDbManager.GetCollection<T>(collectionName);
        }

        public virtual async Task InsertAsync(T data)
        {
            await Collection.InsertOneAsync(data);
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await Collection.Find(_ => true).ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }

        public virtual async Task UpdateAsync(string id, T updatedData)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            UpdateDefinition<T> updateDefinition = CreateUpdateDefinition(updatedData);
            
            if (updateDefinition != null)
            {
                await Collection.UpdateOneAsync(filter, updateDefinition);
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
            var result = await Collection.DeleteOneAsync(filter);

            return result.IsAcknowledged && result.DeletedCount > 0;
        }
}