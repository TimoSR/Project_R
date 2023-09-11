using x_endpoints.Persistence._Interfaces;
using x_endpoints.Persistence.MongoDB;

namespace x_endpoints.Helpers;

public interface IServiceDependencies
{
    ICacheManager CacheManager { get; }
    IEventManager EventManager { get; }
}