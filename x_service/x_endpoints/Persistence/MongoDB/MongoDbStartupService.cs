using MongoDB.Bson;
using MongoDB.Driver;

namespace x_endpoints.Persistence.MongoDB;

public class MongoDbStartupService : IHostedService
{
    private readonly IMongoClient _client;

    public MongoDbStartupService(IMongoClient client)
    {
        _client = client;
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