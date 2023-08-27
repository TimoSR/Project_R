using MongoDB.Driver;
using x_endpoints.Persistence.Google_PubSub;
using x_endpoints.Persistence.MongoDB;
using x_endpoints.Tools.EventMessageBuilders;
using x_endpoints.Tools.EventMessageBuilders.Types;
using x_endpoints.Tools.MessageBuilders;

namespace x_endpoints.Registration.Services;

public abstract class BaseService<T>
{
        private readonly IMongoCollection<T> _collection;
        private readonly PubServices _pubServices;
        private readonly IMessageBuilder<T> _messageBuilder;
        private readonly string _serviceName;
        private readonly string _topicName;

        protected BaseService(
            MongoDbService dbService, 
            string collectionName, 
            PubServices pubServices = null,
            string topicName = null,
            IMessageBuilder<T> messageBuilder = null
            )
        {
            _collection = dbService.GetDefaultDatabase().GetCollection<T>(collectionName);
            _serviceName = DotNetEnv.Env.GetString("SERVICE_NAME");
            _pubServices = pubServices;
            _topicName = topicName;
            _messageBuilder = messageBuilder;
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
        
        public async Task PublishEventAsync(object payload)
        {

            if(_pubServices == null || _messageBuilder == null)
            {
                Console.WriteLine("/n PublishEventAsync called without required dependencies. Operation skipped.");
                return;
            }

            var topicID = _pubServices.GenerateTopicID(_serviceName, _topicName);
            var message = _messageBuilder.BuildMessage(payload);
            
            if (!string.IsNullOrEmpty(message))
            {
                await _pubServices.PublishMessageAsync(topicID, message);
            }
        }
}