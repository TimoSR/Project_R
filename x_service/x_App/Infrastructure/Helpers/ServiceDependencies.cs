using x_endpoints.Infrastructure.Persistence._Interfaces;

namespace x_endpoints.Infrastructure.Helpers;

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