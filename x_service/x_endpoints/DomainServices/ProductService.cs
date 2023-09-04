using x_endpoints.DomainModels;
using x_endpoints.Persistence.Google_PubSub;
using x_endpoints.Persistence.MongoDB;
using x_endpoints.Persistence.Redis;

namespace x_endpoints.DomainServices;

public class ProductService : BaseService<Product>
{

    private readonly PubSubEventPublisher _eventPublisher;
    private readonly RedisService _redisService;
    
    public ProductService(
        MongoDbManager dbManager,
        PubSubEventPublisher eventPublisher) : base(dbManager, "Products")
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
}