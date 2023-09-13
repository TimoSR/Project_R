using x_endpoints.Infrastructure.Persistence._Interfaces;

namespace x_endpoints.Infrastructure.Helpers;

public interface IServiceDependencies
{
    ICacheManager CacheManager { get; }
    IEventManager EventManager { get; }
}