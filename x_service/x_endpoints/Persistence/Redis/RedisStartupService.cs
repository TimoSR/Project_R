namespace x_endpoints.Persistence.Redis;

public class RedisStartupService : IHostedService
{
    private readonly RedisService _redisService;
    
    public RedisStartupService(RedisService redisService)
    {
        _redisService = redisService;
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