using MongoDB.Driver;

namespace x_endpoints.Persistence._Interfaces;

public interface IMongoDbManager
{
    IMongoClient GetClient();
    IMongoDatabase GetDatabase();
    IMongoCollection<T> GetCollection<T>(string collectionName);
}