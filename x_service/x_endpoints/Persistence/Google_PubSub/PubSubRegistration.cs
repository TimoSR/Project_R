using System.Collections;
using Google.Cloud.PubSub.V1;
using Grpc.Core;

namespace x_endpoints.Persistence.Google_PubSub;

public static class PubSubRegistration
{
    public static IServiceCollection AddPubSubServices(this IServiceCollection services, IConfiguration configuration)
    {
        DotNetEnv.Env.Load();

        var publisherConfig = new PublisherConfig
        {
            ProjectId = DotNetEnv.Env.GetString("PROJECT_ID"),
            Topics = GetTopicList()
        };

        services.AddSingleton(publisherConfig);
        services.AddSingleton<PubSubService>();
        
        // Ping test
        try
        {
            var topicName = TopicName.FromProjectTopic("[Your Project ID]", "nonexistent_topic");
            PublisherServiceApiClient.Create().GetTopic(topicName);
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            // Expected exception, means that the service is reachable
            Console.WriteLine("\nPinged your deployment. You successfully connected to Google Pub/Sub!\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n{ex}");
        }

        return services;
    }

    private static List<string> GetTopicList()
    {
        var topicList = new List<string>();

        foreach (DictionaryEntry variable in Environment.GetEnvironmentVariables())
        {
            if (variable.Key.ToString().StartsWith("TOPIC_"))
            {
                topicList.Add(variable.Value.ToString());
            }
        }

        return topicList;
    }
}