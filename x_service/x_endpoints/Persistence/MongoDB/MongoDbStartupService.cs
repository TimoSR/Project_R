using MongoDB.Bson;
using MongoDB.Driver;

namespace x_endpoints.Persistence.MongoDB;

public class MongoDbStartupService : IHostedService
{
    private readonly IMongoClient _client;
    private readonly MongoDbService _mongoDbService;

    public MongoDbStartupService(IMongoClient client, MongoDbService mongoDbService)
    {
        _client = client;
        _mongoDbService = mongoDbService;
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