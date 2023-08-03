using Google.Cloud.PubSub.V1;
using MongoDB.Driver;
using x_endpoints.Models;
using x_endpoints.Persistence.Google_PubSub;
using x_endpoints.Persistence.MongoDB;

namespace x_endpoints.Services;

public class ProductService
{
    
    private readonly IMongoCollection<Product> _products;
    // private readonly PublisherClient _publisherClient;
    private readonly PublisherServiceApiClient _publisherApiClient;
    private readonly PubSubService _pubSubService;

    // The Created as it will react based on the settings in the project.
    // If there is/not a dependency, it will be injected automatically added/removed.
    public ProductService(MongoDbService dbService, PublisherServiceApiClient publisherApiClient, PubSubService pubSubService)
    {
        _products = dbService.GetDefaultDatabase().GetCollection<Product>("Products");
        _publisherApiClient = publisherApiClient;
        _pubSubService = pubSubService;
        //_publisherClient = publisherClient;
    }

     public async Task InsertProduct(Product product)
    {
        await _products.InsertOneAsync(product);

        // Publish a message after inserting a product.
        await _pubSubService.PublishMessageAsync("TOPIC_PRODUCT_UPDATES_V1", $"New product: {product.Name}");

        Console.WriteLine("Inserted Something");
    }


    public List<Product> Get() => _products.Find(product => true).ToList();

    // Add other CRUD operations here...
}