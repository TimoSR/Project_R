using Google.Cloud.PubSub.V1;

namespace x_endpoints.Persistence.Google_PubSub;

public class PubSubStartupService : IHostedService
{
    
    //private readonly PublisherServiceApiClient _client;
    private readonly PubServices _pubServices;
    private readonly SubServices _subServices;

    public PubSubStartupService(IServiceProvider serviceProvider)
    {
        // This makes the Publisher & Subscriber Optional
        _pubServices = serviceProvider.GetService<PubServices>();
        _subServices = serviceProvider.GetService<SubServices>();
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