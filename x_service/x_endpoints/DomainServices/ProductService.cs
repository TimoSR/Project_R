using x_endpoints.DomainModels;
using x_endpoints.Helpers;
using x_endpoints.Persistence._Interfaces;
using x_endpoints.Persistence.Google_PubSub;
using x_endpoints.Persistence.MongoDB;
using x_endpoints.Persistence.Redis;

namespace x_endpoints.DomainServices;

public class ProductService : BaseService<Product>
{
    private readonly IEventManager _eventManager;
    private readonly ICacheManager _cacheManager;
    
    public ProductService(IServiceDependencies dependencies) : base(dependencies.MongoDbManager, "Products")
    {
        _eventManager = dependencies.EventManager;
        _cacheManager = dependencies.CacheManager;
    }

    public override async Task InsertAsync(Product data)
    {
        await Collection.InsertOneAsync(data);
        await _eventManager.PublishJsonEventAsync(data);
        await _eventManager.PublishProtobufEventAsync(data);
    }
}