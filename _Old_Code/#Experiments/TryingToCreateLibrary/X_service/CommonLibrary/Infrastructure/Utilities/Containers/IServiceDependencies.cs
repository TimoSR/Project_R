using CommonLibrary.Infrastructure.Persistence._Interfaces;

namespace CommonLibrary.Infrastructure.Utilities.Containers;

public interface IServiceDependencies
{
    ICacheManager CacheManager { get; }
    IEventPublisher EventPublisher { get; }
}