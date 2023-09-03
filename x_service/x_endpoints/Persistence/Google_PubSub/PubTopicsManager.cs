

using System.Collections;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using x_endpoints.Persistence.StartUp;
using x_endpoints.Tools.Serializers;

namespace x_endpoints.Persistence.Google_PubSub;

public class PubTopicsManager
{
    private readonly PublisherServiceApiClient _publisherClient;
    private readonly string _projectId;
    private readonly string _serviceName;
    private readonly string _environment;
    private readonly IDictionary _environmentVariables;

    public PubTopicsManager(Configuration config, PublisherServiceApiClient publisherClient)
    {
        _projectId = config.ProjectId;
        _serviceName = config.ServiceName;
        _environment = config.Environment;
        _environmentVariables = config.EnvironmentVariables;
        _publisherClient = publisherClient;
        //IfDevelopment();
        CreateTopics();
        ListAllTopicNames();
    }

    private void IfDevelopment()
    {

        var serviceName = _serviceName;
        
        if (_environment == "Development")
        {
            Console.WriteLine("\nDeleting Topics due to ENV: Development...");

            // Get all environment variables
            var environmentVariables = _environmentVariables;

            foreach (var key in environmentVariables.Keys)
            {
                // Check if the environment variable starts with 'TOPIC_'
                if (key.ToString().StartsWith("TOPIC_"))
                {
                    // Get the topic name
                    var topicName = $"{serviceName}-{environmentVariables[key].ToString()}";
                
                    // Delete the topic
                    var existingTopic = _publisherClient.GetTopic(new TopicName(_projectId, topicName));
                    if (existingTopic != null)
                    {
                        _publisherClient.DeleteTopic(existingTopic.TopicName);
                    }
                }
            }
            
        }
    }

    private void CreateTopics()
    {
        // Get all environment variables
        var environmentVariables = _environmentVariables;
        var serviceName = _serviceName;
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
                _publisherClient.GetTopic(topicName);
            } 
            catch (Grpc.Core.RpcException e) when (e.Status.StatusCode == Grpc.Core.StatusCode.NotFound)
            {
                _publisherClient.CreateTopic(topicName);
            }
        }
    }

    private async Task CreateTopicsAsync()
    {
        // Get all environment variables
        var environmentVariables = Environment.GetEnvironmentVariables();
        var serviceName = _serviceName;
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
                await _publisherClient.GetTopicAsync(topicName);
            } 
            catch (Grpc.Core.RpcException e) when (e.Status.StatusCode == Grpc.Core.StatusCode.NotFound)
            {
                await _publisherClient.CreateTopicAsync(topicName);
            }
        });

        await Task.WhenAll(tasks);
    }

    private void ListAllTopicNames()
    {
        ProjectName projectName = new ProjectName(_projectId);

        var allTopics = _publisherClient.ListTopics(projectName);

        Console.WriteLine("\nTopics in PubSub:\n");

        foreach (var topic in allTopics)
        {
            Console.WriteLine($"Topic: {topic.TopicName}");
        }
    }
}