using Domain.Common.DomainModels;
using Domain.Common.IRepositories;
using Domain.DomainModels;
using Infrastructure.Persistence._Interfaces;
using Infrastructure.Persistence.MongoDB;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class ProductRepository : MongoRepository<Product>, IProductRepository
{
    public ProductRepository(IMongoDbManager dbManager, ILogger<ProductRepository> logger) : base(dbManager, logger)
    {
    }

    public async Task<Product> GetProductsByNameAsync(string name)
    {
        try
        {
            var collection = GetCollection();
            return await collection.Find(u => u.Name == name).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            var collectionName = nameof(User) + "s";
            _logger.LogError($"Error retrieving user by email {name} from {collectionName}: {ex.Message}");
            throw;
        }
    }
}