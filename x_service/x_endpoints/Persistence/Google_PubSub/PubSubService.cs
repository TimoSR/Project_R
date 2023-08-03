using Google.Cloud.PubSub.V1;
using Google.Protobuf;

namespace x_endpoints.Persistence.Google_PubSub;

public class PubSubService
{
    private readonly PublisherServiceApiClient _publisherService;
    private readonly string _projectId;

    public PubSubService(PublisherServiceApiClient publisherService, string projectId)
    {
        _publisherService = publisherService;
        _projectId = projectId;
    }

    public async Task PublishMessageAsync(string topicId, string message)
    {
        var topicName = TopicName.FromProjectTopic(_projectId, topicId);
        var pubsubMessage = new PubsubMessage
        {
            Data = ByteString.CopyFromUtf8(message)
        };
        await _publisherService.PublishAsync(topicName, new[] { pubsubMessage });
    }
}