using StackExchange.Redis;
using x_endpoints.Persistence._Interfaces;

namespace x_endpoints.Persistence.Redis;

public class RedisManager : ICacheManager
{
    private readonly IConnectionMultiplexer _redisConnection;

    public RedisManager(IConnectionMultiplexer redisConnection)
    {
        _redisConnection = redisConnection;
    }

    public async Task SetValue(string key, string value)
    {
        var db = _redisConnection.GetDatabase();
        await db.StringSetAsync(key, value);
    }

    public async Task GetValue(string key)
    {
        var db = _redisConnection.GetDatabase();
        await db.StringGetAsync(key);
    }

}