using System.Collections;
using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;

namespace x_endpoints.Persistence.Google_PubSub;

public static class PubSubRegistration {

    public static IServiceCollection AddPubSubServices(this IServiceCollection services)
    {

        DotNetEnv.Env.Load();
        
        var projectId = DotNetEnv.Env.GetString("GOOGLE_CLOUD_PROJECT");
        var topicId = DotNetEnv.Env.GetString("TOPIC_PRODUCT_UPDATES_V1");
        
        TopicName topicName = TopicName.FromProjectTopic(projectId, topicId);

        // Use this if the microservice won't be utlizing scaling to zero. 
        services.AddSingleton<PublisherClient>(sp =>
        {
            Console.WriteLine("\nPublisherClient Created!");
            // Here you should initialize your PublisherClient with the settings you need.
            return PublisherClient.Create(topicName);
        });
        
        services.AddSingleton<PublisherServiceApiClient>(sp => {
            Console.WriteLine("\nPublisherServiceApiClient Created!");
            return PublisherServiceApiClient.Create();
        });
        
        services.AddSingleton(sp => {
            var projectId = DotNetEnv.Env.GetString("GOOGLE_CLOUD_PROJECT");
            var publisherService = sp.GetRequiredService<PublisherServiceApiClient>();
            return new PubSubService(publisherService, projectId);
        });

        return services;
    }
}

public class ProductUpdateService
{
    private readonly PublisherClient _publisherClient;

    public ProductUpdateService(PublisherClient publisherClient)
    {
        _publisherClient = publisherClient;
    }

    // Use the _publisherClient to publish messages.
    public async Task PublishProductUpdateAsync(string message)
    {
        var pubsubMessage = new PubsubMessage
        {
            Data = ByteString.CopyFromUtf8(message),
        };
        string messageId = await _publisherClient.PublishAsync(pubsubMessage);
        Console.WriteLine($"Published message {messageId}");
    }
}