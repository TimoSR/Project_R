using Domain.DomainModels;
using Domain.IRepositories;
using Infrastructure.Persistence._Interfaces;
using Infrastructure.Persistence.MongoDB;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Infrastructure.DomainRepositories;

public class ProductRepository : MongoRepository<Product>, IProductRepository
{
    public ProductRepository(IMongoDbManager dbManager, ILogger<ProductRepository> logger) : base(dbManager, logger)
    {
    }


    public Task<Product> GetProductsByNameAsync(string name)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Product>> GetAllAsync()
    {
        throw new NotImplementedException();
    }
}