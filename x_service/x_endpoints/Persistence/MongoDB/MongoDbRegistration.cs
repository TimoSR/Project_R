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
        var default_database = DotNetEnv.Env.GetString("MONGODB_DEVELOPMENT_DB");

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

            // Print the database names to the console
            Console.WriteLine("Connecting to databases:");
            foreach (var dbName in databases.Keys)
            {
                Console.WriteLine(dbName);
            }

            //Console.WriteLine(environment);

            //Only drop databases if in Development environment
            if (environment.Equals("Development"))
            {
                foreach (var dbName in databases.Keys)
                {

                    //Console.WriteLine(dbName);

                    if(dbName.Equals(default_database)) {

                        // Drop the entire database
                        client.DropDatabase(dbName);

                        Console.WriteLine($"`Database: {dbName} are now cleared!");
                    }
                    
                }
            }

            return new MongoDbService(client, default_database, databases);
        });

        services.AddApplicationServices(); // add this line to register all the services.

        return services;
    }
}