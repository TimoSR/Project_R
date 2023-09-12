using StackExchange.Redis;
using x_endpoints.Helpers;
using x_endpoints.Persistence._Interfaces;
using x_endpoints.Persistence.StartUp;

namespace x_endpoints.Persistence.Redis;

public static class RedisRegistration
{
    public static IServiceCollection AddRedisServices(this IServiceCollection services, Configuration config)
    {

        var redisConnectionString = config.RedisConnectionString;

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

        services.AddSingleton<ICacheManager, RedisManager>();

        return services;
    }
}