

using System.Collections;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using x_endpoints.Tools.Serializers;

namespace x_endpoints.Persistence.Google_PubSub;

public class PubServices
{
    private readonly PublisherServiceApiClient _publisherService;
    private readonly string _projectId;

    public PubServices(PublisherServiceApiClient publisherService, string projectId)
    {
        _publisherService = publisherService;
        _projectId = projectId;
        IfDevelopment();
        CreateTopics();
        ListAllTopicNames();
    }

    private void IfDevelopment() {

        var serviceName = DotNetEnv.Env.GetString("SERVICE_NAME");
        
        if (Environment.GetEnvironmentVariable("ENVIRONMENT") == "Development")
        {
            Console.WriteLine("\nDeleting Topics due to ENV: Development...");

            // Get all environment variables
            var envVars = Environment.GetEnvironmentVariables();

            foreach (var key in envVars.Keys)
            {
                // Check if the environment variable starts with 'TOPIC_'
                if (key.ToString().StartsWith("TOPIC_"))
                {
                    // Get the topic name
                    var topicName = $"{serviceName}-{envVars[key].ToString()}";
                
                    // Delete the topic
                    var existingTopic = _publisherService.GetTopic(new TopicName(_projectId, topicName));
                    if (existingTopic != null)
                    {
                        _publisherService.DeleteTopic(existingTopic.TopicName);
                    }
                }
            }
            
        }
    }

    private void CreateTopics()
    {
        // Get all environment variables
        var environmentVariables = Environment.GetEnvironmentVariables();
        var serviceName = DotNetEnv.Env.GetString("SERVICE_NAME");
        var topics = new List<string>();

        Console.WriteLine("\nCreating Topics:");

        // Filter environment variables starting with "TOPIC_"
        foreach (DictionaryEntry variable in environmentVariables)
        {
            string key = variable.Key.ToString();
            if (key.StartsWith("TOPIC_"))
            {
                Console.WriteLine($"\n{variable.Key}");
                Console.WriteLine($"{variable.Value}");
                var topicID = $"{serviceName}-{variable.Value.ToString()}";
                topics.Add(topicID);
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

    private async Task CreateTopicsAsync()
    {
        // Get all environment variables
        var environmentVariables = Environment.GetEnvironmentVariables();
        var serviceName = DotNetEnv.Env.GetString("SERVICE_NAME");
        var topics = new List<string>();

        Console.WriteLine("\nCreating Topics:");

        // Filter environment variables starting with "TOPIC_"
        foreach (DictionaryEntry variable in environmentVariables)
        {
            string key = variable.Key.ToString();
            if (key.StartsWith("TOPIC_"))
            {
                Console.WriteLine($"\n{variable.Key}");
                Console.WriteLine($"{variable.Value}");
                var topicID = $"{serviceName}-{variable.Value.ToString()}";
                topics.Add(topicID);
            }
        }

        // Create topics if they don't exist
        var tasks = topics.Select(async topicId =>
        {
            var topicName = TopicName.FromProjectTopic(_projectId, topicId);
            try 
            {
                await _publisherService.GetTopicAsync(topicName);
            } 
            catch (Grpc.Core.RpcException e) when (e.Status.StatusCode == Grpc.Core.StatusCode.NotFound)
            {
                await _publisherService.CreateTopicAsync(topicName);
            }
        });

        await Task.WhenAll(tasks);
    }

    private void ListAllTopicNames()
    {
        ProjectName projectName = new ProjectName(_projectId);

        var allTopics = _publisherService.ListTopics(projectName);

        Console.WriteLine("\nTopics in PubSub:\n");

        foreach (var topic in allTopics)
        {
            Console.WriteLine($"Topic: {topic.TopicName}");
        }
    }

    public string GenerateTopicID(string serviceName, string topic){
        var _serviceName = DotNetEnv.Env.GetString(serviceName);
        var _topic = DotNetEnv.Env.GetString(topic);
        return $"{_serviceName}-{_topic}";
    }

    public async Task PublishMessageAsync(string topicId, string eventType, string formattedMessage)
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