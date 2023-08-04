using Google.Api.Gax.ResourceNames;
using Google.Cloud.PubSub.V1;
using MongoDB.Driver;
using x_endpoints.Models;
using x_endpoints.Persistence.Google_PubSub;
using x_endpoints.Persistence.MongoDB;

namespace x_endpoints.Services;

public class ProductService
{
    
    private readonly IMongoCollection<Product> _products;
    private readonly PubSubService _pubSubService;

    // The Created as it will react based on the settings in the project.
    // If there is/not a dependency, it will be injected automatically added/removed.
    public ProductService(MongoDbService dbService, PubSubService pubSubService)
    {
        _products = dbService.GetDefaultDatabase().GetCollection<Product>("Products");
        //_publisherApiClient = publisherApiClient;
        _pubSubService = pubSubService;
        //_publisherClient = publisherClient;
    }

     public async Task InsertProduct(Product product)
    {
        await _products.InsertOneAsync(product);

        var topicID = _pubSubService.GenerateTopicID("SERVICE_NAME", "TOPIC_PRODUCT_UPDATES");

        // Publish a message after inserting a product.
        await _pubSubService.PublishMessageAsync(topicID, $"New product: {product.Name}");

        Console.WriteLine("A Product was inserted into MongoDB!\n");
    }

    public List<Product> Get() => _products.Find(product => true).ToList();

    // Add other CRUD operations here...
}