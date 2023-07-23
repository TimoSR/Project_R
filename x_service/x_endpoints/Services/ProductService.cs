using MongoDB.Driver;
using x_endpoints.Models;
using x_endpoints.Persistence.MongoDB;

namespace x_endpoints.Services;

public class ProductService
{
    private readonly IMongoCollection<Product> _products;

    public ProductService(MongoDbService dbService)
    {
        _products = dbService.GetDatabase("default").GetCollection<Product>("Products");
    }

    public List<Product> Get() => _products.Find(product => true).ToList();

    // Add other CRUD operations here...
}