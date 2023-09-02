using x_endpoints.Persistence.Google_PubSub;
using x_endpoints.Persistence.MongoDB;
using x_endpoints.Persistence.Redis;

namespace x_endpoints.Persistence.StartUp;

public class StartExternalConnections : IHostedService
{
    private readonly MongoDbService _mongoDbService;
    private readonly PubTopicsManager _pubTopicsManager;
    private readonly SubTopicsManager _subTopicsManager;
    private readonly RedisService _redisService;

    public StartExternalConnections(IServiceProvider serviceProvider)
    {
        // This makes the MongoDB Optional
        _mongoDbService = serviceProvider.GetService<MongoDbService>();
        _pubTopicsManager = serviceProvider.GetService<PubTopicsManager>();
        _subTopicsManager = serviceProvider.GetService<SubTopicsManager>();
        _redisService = serviceProvider.GetService<RedisService>();
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