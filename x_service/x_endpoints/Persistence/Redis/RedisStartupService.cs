namespace x_endpoints.Persistence.Redis;

public class RedisStartupService : IHostedService
{
    private readonly RedisService _redisService;
    
    public RedisStartupService(IServiceProvider serviceProvider)
    {

        // This makes Redis Optional
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