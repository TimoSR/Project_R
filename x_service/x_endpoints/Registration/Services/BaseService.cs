using MongoDB.Driver;
using x_endpoints.Persistence.Google_PubSub;
using x_endpoints.Persistence.MongoDB;
using x_endpoints.Tools.Serializers;
using x_endpoints.Tools.Serializers.Types;

namespace x_endpoints.Registration.Services;

public abstract class BaseService<T>
{
        protected readonly IMongoCollection<T> Collection;
        protected readonly PubServices PubServices;
        protected readonly string ServiceName;
        protected readonly string TopicName;
        protected readonly ISerializer<object> JsonSerializer;
        protected readonly ISerializer<object> ProtobufSerializer;

        protected BaseService(
            MongoDbService dbService, 
            string collectionName, 
            PubServices pubServices = null,
            string topicName = null,
            ISerializer<object> jsonSerializer = null,
            ISerializer<object> protobufSerializer = null
            )
        {
            Collection = dbService.GetDefaultDatabase().GetCollection<T>(collectionName);
            ServiceName = DotNetEnv.Env.GetString("SERVICE_NAME");
            PubServices = pubServices;
            TopicName = topicName;
            JsonSerializer = jsonSerializer;
            ProtobufSerializer = protobufSerializer;
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
        
        public async Task PublishEventAsync(object payload, string eventType)
        {
            if(PubServices == null )
            {
                Console.WriteLine("/n PublishEventAsync called without required dependencies. Operation skipped.");
                return;
            }

            var eventMessage = ProtobufSerializer.Serialize(payload);

            var topicID = PubServices.GenerateTopicID(ServiceName, TopicName);
            
            if (!string.IsNullOrEmpty(eventMessage))
            {
                await PubServices.PublishMessageAsync(topicID, eventType, eventMessage);
            }
        }
}