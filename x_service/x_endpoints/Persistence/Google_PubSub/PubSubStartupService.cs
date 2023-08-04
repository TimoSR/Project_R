using Google.Cloud.PubSub.V1;

namespace x_endpoints.Persistence.Google_PubSub;

public class PubSubStartupService : IHostedService
{
    
    private readonly PublisherServiceApiClient _client;
    private readonly PubServices _pubServices;

    public PubSubStartupService(PublisherServiceApiClient client, PubServices pubServices)
    {
        _client = client;
        _pubServices = pubServices;
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