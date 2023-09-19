using Infrastructure.Persistence._Interfaces;
using ServiceLibrary.Persistence._Interfaces;

namespace Infrastructure.Utilities.Containers;

public class ServiceDependencies : IServiceDependencies
{
    public IEventPublisher EventPublisher { get; }
    public ICacheManager CacheManager { get; }
    
    public ServiceDependencies(
        IEventPublisher eventPublisher = null,
        ICacheManager cacheManager = null
    )
    {
        EventPublisher = eventPublisher;
        CacheManager = cacheManager;
    }
}