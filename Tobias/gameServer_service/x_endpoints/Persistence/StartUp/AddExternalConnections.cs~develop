using x_endpoints.Persistence.Google_PubSub;
using x_endpoints.Persistence.MongoDB;
using x_endpoints.Persistence.Redis;

namespace x_endpoints.Persistence.StartUp;

public class AddExternalConnections : IHostedService
{
    private readonly MongoDbService _mongoDbService;
    private readonly PubServices _pubServices;
    private readonly SubServices _subServices;
    private readonly RedisService _redisService;

    public AddExternalConnections(IServiceProvider serviceProvider)
    {
        // This makes the MongoDB Optional
        _mongoDbService = serviceProvider.GetService<MongoDbService>();
        _pubServices = serviceProvider.GetService<PubServices>();
        _subServices = serviceProvider.GetService<SubServices>();
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