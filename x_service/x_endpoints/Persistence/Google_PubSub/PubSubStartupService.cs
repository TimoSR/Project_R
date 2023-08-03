namespace x_endpoints.Persistence.Google_PubSub;

public class PubSubStartupService : IHostedService
{
    private readonly PubSubService _pubSubService;

    public PubSubStartupService(PubSubService pubSubService)
    {
        _pubSubService = pubSubService;
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
