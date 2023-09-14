using x_App.Infrastructure.Persistence._Interfaces;

namespace x_App.Infrastructure.Helpers;

public interface IServiceDependencies
{
    ICacheManager CacheManager { get; }
    IEventManager EventManager { get; }
}