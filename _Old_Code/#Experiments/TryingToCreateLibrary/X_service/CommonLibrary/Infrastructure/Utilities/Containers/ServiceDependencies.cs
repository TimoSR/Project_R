using CommonLibrary.Infrastructure.Persistence._Interfaces;

namespace CommonLibrary.Infrastructure.Utilities.Containers;

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