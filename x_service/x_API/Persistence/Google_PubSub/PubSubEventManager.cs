using System.Reflection;
using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using x_endpoints.Helpers.Attributes;
using x_endpoints.Persistence._Interfaces;
using x_endpoints.Persistence.StartUp;
using x_endpoints.Tools.Serializers;

namespace x_endpoints.Persistence.Google_PubSub;

public class PubSubEventManager : IEventManager
{
    private readonly JsonSerializer _jsonSerializer;
    private readonly ProtobufSerializer _protobufSerializer;
    private readonly PublisherServiceApiClient _publisherService;
    private readonly string _projectId;
    private readonly string _serviceName;

    public PubSubEventManager(
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

    /*
     * This Design Enables the method to react on the Input Type.
     */
    public async Task PublishJsonEventAsync<TEvent>(TEvent eventMessage)
    {
        var eventType = typeof(TEvent);
        var serializedMessage = _jsonSerializer.Serialize(eventMessage);
        string topicId = GenerateTopicID(eventType);
        await PublishMessageAsync(topicId, eventType.Name, serializedMessage);
    }

    public async Task PublishProtobufEventAsync<TEvent>(TEvent eventMessage)
    {
        var eventType = typeof(TEvent);
        var serializedMessage = _protobufSerializer.Serialize(eventMessage);
        string topicId = GenerateTopicID(eventType);
        await PublishMessageAsync(topicId, eventType.Name, serializedMessage);
    }

    private string GenerateTopicID(Type eventType)
    {
        var topicAttribute = eventType.GetCustomAttribute<TopicNameAttribute>();

        // If the class doesn't have the attribute, fall back to using the class name
        var eventName = topicAttribute?.Name ?? eventType.Name;

        return $"{_serviceName}-{eventName}";
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