using MongoDB.Driver;

namespace x_endpoints.Persistence.MongoDB;

public class MongoDbService
{
    private readonly IMongoClient _client;

    private readonly string _default_database;
    private readonly IDictionary<string, string> _databaseNames;

    public MongoDbService(IMongoClient client, string default_database, IDictionary<string, string> databaseNames)
    {
        _client = client;
        _default_database = default_database; 
        _databaseNames = databaseNames;
    } 

    public IMongoDatabase GetDefaultDatabase() {
        return _client.GetDatabase(_default_database);
    }

    public IMongoDatabase GetDatabase(string key)
    {
        if (!_databaseNames.TryGetValue(key, out var databaseName))
        {
            throw new KeyNotFoundException($"No database configuration found for key: {key}");
        }
        return _client.GetDatabase(databaseName);
    }
}