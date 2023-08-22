using StackExchange.Redis;

namespace x_endpoints.Persistence.Redis;

public static class RedisRegistration
{
    public static IServiceCollection AddRedisServices(this IServiceCollection services)
    {
        DotNetEnv.Env.Load();

        var redisConnectionString = DotNetEnv.Env.GetString("REDIS_CONNECTION_STRING");

        services.AddSingleton<IConnectionMultiplexer>(serviceProvider =>
        {
            var client = ConnectionMultiplexer.Connect(redisConnectionString);

            // Check the connection status
            if (!client.IsConnected)
            {
                Console.WriteLine("Failed to establish a connection to Redis.");
                throw new InvalidOperationException("Cannot start application without Redis connection.");
            }

            Console.WriteLine("\n###################################");

            var db = client.GetDatabase();
            var pong = db.Execute("PING");
            
            if (pong.ToString() == "PONG")
            {
                Console.WriteLine("\nYou successfully connected to Redis!");
            }
            else
            {
                throw new InvalidOperationException("Unexpected response from Redis.");
            }

            return client;
        });

        services.AddSingleton<RedisService>();

        return services;
    }
}