using x_endpoints.Persistence._Interfaces;
using x_endpoints.Persistence.MongoDB;

namespace x_endpoints.Helpers;

public class ServiceDependencies : IServiceDependencies
{
    public IMongoDbManager MongoDbManager { get; }
    public IEventManager EventManager { get; }
    public ICacheManager CacheManager { get; }
    
    public ServiceDependencies(
        IMongoDbManager mongoDbManager, 
        IEventManager eventManager,
        ICacheManager cacheManager
    )
    {
        MongoDbManager = mongoDbManager;
        EventManager = eventManager;
        CacheManager = cacheManager;
    }
}