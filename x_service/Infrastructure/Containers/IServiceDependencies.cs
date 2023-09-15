using ServiceLibrary.Persistence._Interfaces;

namespace x_App.Infrastructure.Containers;

public interface IServiceDependencies
{
    ICacheManager CacheManager { get; }
    IEventPublisher EventPublisher { get; }
}