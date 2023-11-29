using MongoDB.Driver;

namespace CommonLibrary.Infrastructure.Persistence._Interfaces;

public interface IMongoDbManager
{
    IMongoClient GetClient();
    IMongoDatabase GetDatabase();
    IMongoCollection<T> GetCollection<T>(string collectionName);
}