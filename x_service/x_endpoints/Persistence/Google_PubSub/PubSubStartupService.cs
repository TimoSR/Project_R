using Google.Cloud.PubSub.V1;
using x_endpoints.Persistence.MongoDB;

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
        throw new NotImplementedException();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}