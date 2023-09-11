using x_endpoints.Persistence._Interfaces;
using x_endpoints.Persistence.MongoDB;

namespace x_endpoints.Helpers;

public class ServiceDependencies : IServiceDependencies
{
    public IEventManager EventManager { get; }
    public ICacheManager CacheManager { get; }
    
    public ServiceDependencies(
        IEventManager eventManager = null,
        ICacheManager cacheManager = null
    )
    {
        EventManager = eventManager;
        CacheManager = cacheManager;
    }
}