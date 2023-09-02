using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using x_endpoints.Persistence.StartUp;
using x_endpoints.Tools.Serializers;

namespace x_endpoints.Persistence.Google_PubSub;

public class PubSubEventPublisher
{
    private readonly JsonSerializer _jsonSerializer;
    private readonly ProtobufSerializer _protobufSerializer;
    private readonly PublisherServiceApiClient _publisherService;
    private readonly string _projectId;
    private readonly string _serviceName;

    public PubSubEventPublisher(
        Configuration config,
        PublisherServiceApiClient publisherService,
        JsonSerializer jsonSerializer,
        ProtobufSerializer protobufSerializer)
    {
        _projectId = config.ProjectId;
        _serviceName = config.ServiceName;
        _publisherService = publisherService;
        _jsonSerializer = jsonSerializer;
        _protobufSerializer = protobufSerializer;
    }

    public async Task PublishJsonEventAsync<TEvent>(TEvent eventMessage)
    {
        var eventName = typeof(TEvent).Name;
        var serializedMessage = _jsonSerializer.Serialize(eventMessage);
        string topicId = GenerateTopicID(eventName);
        await PublishMessageAsync(topicId, eventName, serializedMessage);
    }
    
    public async Task PublishProtobufEventAsync<TEvent>(TEvent eventMessage)
    {
        var eventName = typeof(TEvent).Name;
        var serializedMessage = _protobufSerializer.Serialize(eventMessage);
        string topicId = GenerateTopicID(eventName);
        await PublishMessageAsync(topicId, eventName, serializedMessage);
    }

    private string GenerateTopicID(string eventName)
    {
        var serviceName = _serviceName;  // Assuming you have SERVICE_NAME in your env
        var topicID = $"{serviceName}-{eventName}";
        //Console.WriteLine(topicID);
        return topicID;
    }

    private async Task PublishMessageAsync(string topicId, string eventType, string formattedMessage)
    {
        var topicName = TopicName.FromProjectTopic(_projectId, topicId);

        PubsubMessage pubsubMessage = new PubsubMessage
        {
            Data = ByteString.CopyFromUtf8(formattedMessage),
            Attributes =
            {
                { "description", $"Message for event type: {eventType}" },
                { "eventType", eventType }
            }
        };

        await _publisherService.PublishAsync(topicName, new[] { pubsubMessage });
    }
}