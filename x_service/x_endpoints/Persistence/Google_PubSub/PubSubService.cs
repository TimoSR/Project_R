using System.Collections;
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
        CreateTopics();
    }

    private void CreateTopics()
    {
        // Get all environment variables
        var environmentVariables = Environment.GetEnvironmentVariables();
        var topics = new List<string>();

        // Filter environment variables starting with "TOPIC_"
        foreach (DictionaryEntry variable in environmentVariables)
        {
            string key = variable.Key.ToString();
            if (key.StartsWith("TOPIC_"))
            {
                Console.WriteLine(variable.Key);
                Console.WriteLine(variable.Value);
                topics.Add(variable.Value.ToString());
            }
        }

        // Create topics if they don't exist
        foreach (var topicId in topics)
        {
            var topicName = TopicName.FromProjectTopic(_projectId, topicId);
            try 
            {
                _publisherService.GetTopic(topicName);
            } 
            catch (Grpc.Core.RpcException e) when (e.Status.StatusCode == Grpc.Core.StatusCode.NotFound)
            {
                _publisherService.CreateTopic(topicName);
            }
        }
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