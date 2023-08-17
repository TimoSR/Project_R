using MongoDB.Bson;
using MongoDB.Driver;

namespace x_endpoints.Persistence.MongoDB;

public class MongoDbStartupService : IHostedService
{
    private readonly MongoDbService _mongoDbService;

    public MongoDbStartupService(IServiceProvider serviceProvider)
    {
        // This makes the MongoDB Optional
        _mongoDbService = serviceProvider.GetService<MongoDbService>();
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