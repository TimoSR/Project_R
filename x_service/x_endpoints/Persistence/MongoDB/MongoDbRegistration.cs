using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using x_endpoints.Services;

namespace x_endpoints.Persistence.MongoDB;

public static class MongoDbRegistration
{
    public static IServiceCollection AddMongoDBServices(this IServiceCollection services, IConfiguration configuration)
    {
        DotNetEnv.Env.Load();

        var connectionString = DotNetEnv.Env.GetString("MONGODB_CONNECTION_STRING");
        var environment = DotNetEnv.Env.GetString("ENVIRONMENT");

        services.AddSingleton<IMongoClient>(sp =>
        {
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            var client = new MongoClient(settings);

            // Ping test
            try 
            {
                var result = client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
                Console.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");
            } 
            catch (Exception ex) 
            {
                Console.WriteLine(ex);
            }

            return client;
        });

        services.AddSingleton<MongoDbService>(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            var databases = client.ListDatabaseNames().ToEnumerable().ToDictionary(name => name, name => name);

            // Only drop databases if in Development environment
            if (environment == "Development")
            {
                foreach (var dbName in databases.Keys)
                {
                    var db = client.GetDatabase(dbName);

                    // Drop all collections within the database
                    var collections = db.ListCollectionNames().ToList();
                    foreach (var collection in collections)
                    {
                        db.DropCollection(collection);
                    }
                }
            }

            return new MongoDbService(client, databases);
        });

        services.AddApplicationServices(); // add this line to register all the services.

        return services;
    }
}