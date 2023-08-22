using MongoDB.Driver;
using x_endpoints.DomainModels;
using x_endpoints.Persistence.Google_PubSub;
using x_endpoints.Persistence.MongoDB;
using x_endpoints.Persistence.ServiceRegistration;

namespace x_endpoints.DomainServices;

public class ProductService : BaseService<Product>
{
    private readonly PubServices _pubServices;
    //private readonly SubServices _subServices;
    //private readonly RedisService _redisService;

    // The Created as it will react based on the settings in the project.
    // If there is/not a dependency, it will be injected automatically added/removed.
    public ProductService(MongoDbService dbService, PubServices pubServices) : base(dbService, "Products")
    {
        _pubServices = pubServices;
        //_subServices = subServices;
        //_redisService = redisService;
    }

    public override async Task InsertAsync(Product data)
    {

        await _collection.InsertOneAsync(data);        
    
        var topicID = _pubServices.GenerateTopicID("SERVICE_NAME", "TOPIC_PRODUCT_UPDATES");
        //Console.WriteLine(topicID);
    
        // Publish a message after inserting a product.
        await _pubServices.PublishMessageAsync(topicID, $"New product: {data.Name}");
    
        //await _redisService.SetValue("1", "test");
    }

    // Add other CRUD operations here...
}