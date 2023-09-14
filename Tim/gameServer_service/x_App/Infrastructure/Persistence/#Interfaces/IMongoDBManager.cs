using MongoDB.Driver;

namespace x_endpoints.Infrastructure.Persistence._Interfaces;

public interface IMongoDbManager
{
    IMongoClient GetClient();
    IMongoDatabase GetDatabase();
    IMongoCollection<T> GetCollection<T>(string collectionName);
}