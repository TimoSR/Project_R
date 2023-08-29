using MongoDB.Driver;
using x_endpoints.DomainModels;
using x_endpoints.Persistence.Google_PubSub;
using x_endpoints.Persistence.MongoDB;
using x_endpoints.Registration.Services;
using x_endpoints.Tools.Serializers;

namespace x_endpoints.DomainServices;

public class ProductService : BaseService<Product>
{
    private readonly PubServices _pubServices;
    //private readonly SubServices _subServices;
    //private readonly RedisService _redisService;
    private JsonSerializer<object> _jsonSerializer;
    
    //The Model needs to follow the Product Attribute!!!
    private ProtobufSerializer<Product> _protobufSerializer;

    // The Created as it will react based on the settings in the project.
    // If there is/not a dependency, it will be injected automatically added/removed.
    public ProductService(
        MongoDbService dbService,
        PubServices pubServices, 
        JsonSerializer<object> jsonSerializer,
        ProtobufSerializer<Product> protobufSerializer) : base(dbService, "Products")
    {
        _pubServices = pubServices;
        //_subServices = subServices;
        //_redisService = redisService;
        _jsonSerializer = jsonSerializer;
        _protobufSerializer = protobufSerializer;
    }

    public override async Task InsertAsync(Product data)
    {

        await Collection.InsertOneAsync(data);        
    
        var topicID = _pubServices.GenerateTopicID("SERVICE_NAME", "TOPIC_PRODUCT_UPDATES");
        //Console.WriteLine(topicID);

        var eventMessageJson = _jsonSerializer.Serialize(data);
        var eventMessageProtobuf = _protobufSerializer.Serialize(data);
    
        // Publish a message after inserting a product.
        await _pubServices.PublishMessageAsync(topicID, "New Product", eventMessageJson);
        await _pubServices.PublishMessageAsync(topicID, "New Product", eventMessageProtobuf);
    
        //await _redisService.SetValue("1", "test");
    }

    // Add other CRUD operations here...
}