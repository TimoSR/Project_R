using MongoDB.Driver;

namespace x_endpoints.Persistence.MongoDB;

public class MongoDbManager
{
    private readonly IMongoClient _client;
    private readonly string _defaultDatabase;
    private readonly IDictionary<string, string> _databaseNames;

    public MongoDbManager(IMongoClient client, string defaultDatabase, IDictionary<string, string> databaseNames)
    {
        _client = client;
        _defaultDatabase = defaultDatabase; 
        _databaseNames = databaseNames;
    } 

    public IMongoDatabase GetDefaultDatabase() {
        return _client.GetDatabase(_defaultDatabase);
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