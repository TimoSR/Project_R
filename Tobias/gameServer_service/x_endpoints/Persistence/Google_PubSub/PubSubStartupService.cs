using Google.Cloud.PubSub.V1;

namespace x_endpoints.Persistence.Google_PubSub;

public class PubSubStartupService : IHostedService
{
    
    private readonly PublisherServiceApiClient _client;
    private readonly PubSubService _pubSubService;

    public PubSubStartupService(PublisherServiceApiClient client, PubSubService pubSubService)
    {
        _client = client;
        _pubSubService = pubSubService;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Since the work is done, return a completed Task
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        // Since the work is done, return a completed Task
        return Task.CompletedTask;
    }
}