using ServiceLibrary.Persistence._Interfaces;

namespace x_App.Infrastructure.Containers;

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