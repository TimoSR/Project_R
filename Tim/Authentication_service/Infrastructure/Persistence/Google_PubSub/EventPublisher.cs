using System.Reflection;
using _CommonLibrary.Patterns.RegistrationHooks.Events._Attributes;
using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using Infrastructure.Persistence._Interfaces;
using Infrastructure.Utilities;
using Infrastructure.Utilities._Interfaces;

namespace Infrastructure.Persistence.Google_PubSub;

public class EventPublisher : IEventPublisher
{
    private readonly IJsonSerializer _jsonSerializer;
    private readonly IProtobufSerializer _protobufSerializer;
    private readonly PublisherServiceApiClient _publisherService;
    private readonly string _projectId;
    private readonly string _serviceName;

    public EventPublisher(
        IConfiguration config,
        PublisherServiceApiClient publisherService,
        IJsonSerializer jsonSerializer,
        IProtobufSerializer protobufSerializer)
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