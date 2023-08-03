namespace x_endpoints.Persistence.Google_PubSub;

using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using System;
using System.Text.Json;

public class PubSubService
{
    private readonly PublisherServiceApiClient _publisherService;
    private readonly string _projectId;

    public PubSubService(PublisherServiceApiClient publisherService, string projectId)
    {
        _publisherService = publisherService;
        _projectId = projectId;
    }

    public Task<PublishResponse> PublishMessage<T>(string topicName, T message)
    {
        var topic = new TopicName(_projectId, topicName);
        var pubsubMessage = new PubsubMessage
        {
            // Convert the message to a JSON string, then to bytes, which can be used to create the PubsubMessage
            Data = ByteString.CopyFromUtf8(JsonSerializer.Serialize(message))
        };

        // Publish the message to the topic and return the Task.
        // The caller can decide whether to await this task or not.
        return _publisherService.PublishAsync(topic, new[] { pubsubMessage });
    }
}
