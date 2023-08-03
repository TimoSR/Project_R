using Google.Cloud.PubSub.V1;
using MongoDB.Driver;
using x_endpoints.Models;
using x_endpoints.Persistence.MongoDB;

namespace x_endpoints.Services;

public class ProductService
{
    
    private readonly IMongoCollection<Product> _products;
    private readonly dynamic _publisherClient;
    private readonly dynamic _publisherApiClient;

    // The Created as it will react based on the settings in the project.
    // If there is/not a dependency, it will be injected automatically added/removed.
    public ProductService(MongoDbService dbService, PublisherServiceApiClient publisherApiClient, PublisherClient publisherClient)
    {
        _products = dbService.GetDefaultDatabase().GetCollection<Product>("Products");
        _publisherClient = publisherClient;
        _publisherApiClient = publisherApiClient;
    }

    public void InsertProduct(Product product)
    {
        _products.InsertOne(product);
    }

    public List<Product> Get() => _products.Find(product => true).ToList();

    // Add other CRUD operations here...
}