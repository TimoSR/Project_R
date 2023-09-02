using MongoDB.Driver;
using x_endpoints.DomainModels;
using x_endpoints.Persistence.Google_PubSub;
using x_endpoints.Persistence.MongoDB;
using x_endpoints.Registration.Services;
using x_endpoints.Tools.Serializers;

namespace x_endpoints.DomainServices;

public class ProductService : BaseService<Product>
{

    private readonly PubSubEventPublisher _eventPublisher;
    //private readonly RedisService _redisService;

    // The Created as it will react based on the settings in the project.
    // If there is/not a dependency, it will be injected automatically added/removed.
    public ProductService(
        MongoDbService dbService,
        PubSubEventPublisher eventPublisher) : base(dbService, "Products")
    {
        _eventPublisher = eventPublisher;
        //_redisService = redisService;
    }

    public override async Task InsertAsync(Product data)
    {

        await Collection.InsertOneAsync(data);

        await _eventPublisher.PublishJsonEventAsync(data);
        await _eventPublisher.PublishProtobufEventAsync(data);
    }

    // Add other CRUD operations here...
}