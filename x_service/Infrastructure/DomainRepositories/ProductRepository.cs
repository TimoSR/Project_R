using Domain.DomainModels;
using Domain.IRepositories;
using Infrastructure.Persistence._Interfaces;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using ServiceLibrary.DomainRepositories._Abstract;
using ServiceLibrary.Persistence._Interfaces;

namespace Infrastructure.DomainRepositories;

public class ProductRepository : MongoRepository<Product>, IProductRepository
{
    public ProductRepository(IMongoDbManager dbManager, ILogger<ProductRepository> logger) 
        : base(dbManager, logger)
    {
    }

    public async Task<IEnumerable<Product>> GetProductsByNameAsync(string productName)
    {
        try
        {
            var collection = GetCollection();
            return await collection.Find(p => p.Name == productName).ToListAsync();
        }
        catch (Exception ex)
        {
            var collectionName = typeof(Product).Name + "s";
            _logger.LogError($"Error retrieving products by name {productName} from {collectionName}: {ex.Message}");
            throw;
        }
    }
}
