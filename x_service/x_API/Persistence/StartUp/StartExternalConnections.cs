using x_endpoints.Persistence._Interfaces;
using x_endpoints.Persistence.Google_PubSub;
using x_endpoints.Persistence.MongoDB;
using x_endpoints.Persistence.Redis;

namespace x_endpoints.Persistence.StartUp;

public class StartExternalConnections : IHostedService
{
    private readonly IMongoDbManager _mongoDbManager;
    private readonly PubTopicsManager _pubTopicsManager;
    private readonly SubTopicsManager _subTopicsManager;
    private readonly ICacheManager _cacheManager;

    public StartExternalConnections(IServiceProvider serviceProvider)
    {
        // This makes the MongoDB Optional
        _mongoDbManager = serviceProvider.GetService<IMongoDbManager>();
        _pubTopicsManager = serviceProvider.GetService<PubTopicsManager>();
        _subTopicsManager = serviceProvider.GetService<SubTopicsManager>();
        _cacheManager = serviceProvider.GetService<ICacheManager>();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Since the work is done, return a completed Task
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        // There is nothing to do on stop, so just return a completed Task
        return Task.CompletedTask;
    }
}