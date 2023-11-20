using CommonLibrary.Infrastructure.Persistence._Interfaces;
using StackExchange.Redis;

namespace CommonLibrary.Infrastructure.Persistence.Redis;

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