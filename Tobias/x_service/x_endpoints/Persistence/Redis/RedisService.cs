using StackExchange.Redis;

namespace x_endpoints.Persistence.Redis;

public class RedisService
{
    private readonly IConnectionMultiplexer _redisConnection;

    public RedisService(IConnectionMultiplexer redisConnection)
    {
        _redisConnection = redisConnection;
    }

    public async Task SetValue(string key, string value)
    {
        var db = _redisConnection.GetDatabase();
        await db.StringSetAsync(key, value);
    }

    public string GetValue(string key)
    {
        var db = _redisConnection.GetDatabase();
        return db.StringGet(key);
    }

}