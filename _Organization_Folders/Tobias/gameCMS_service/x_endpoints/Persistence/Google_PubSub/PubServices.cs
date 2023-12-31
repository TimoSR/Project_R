using System.Collections;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.PubSub.V1;
using Google.Protobuf;

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

        CreateTopics();
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

    public async Task PublishMessageAsync(string topicId, string message)
    {
        var topicName = TopicName.FromProjectTopic(_projectId, topicId);

        PubsubMessage pubsubMessage = new PubsubMessage
        {
            // The data is any arbitrary ByteString. Here, we're using text.
            Data = ByteString.CopyFromUtf8("Hello, Pubsub"),
            // The attributes provide metadata in a string-to-string dictionary.
            Attributes =
            {
                { "description", "Simple text message" }
            }
        };

        // Publish a message to the topic using PublisherClient.
        await _publisherService.PublishAsync(topicName, new[] { pubsubMessage });
    }
}