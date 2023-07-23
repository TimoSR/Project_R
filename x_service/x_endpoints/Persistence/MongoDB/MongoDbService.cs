using MongoDB.Driver;

namespace x_endpoints.Persistence.MongoDB;

public class MongoDbService
{
    private readonly IMongoClient _client;
    private readonly IDictionary<string, string> _databaseNames;

    public MongoDbService(IMongoClient client, IDictionary<string, string> databaseNames)
    {
        _client = client;
        _databaseNames = databaseNames;
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