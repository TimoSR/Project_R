using System.Collections;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;

namespace x_endpoints.Persistence.Google_PubSub;

public static class PubSubRegistration {

    public static IServiceCollection AddPublisherServices(this IServiceCollection services)
    {

        DotNetEnv.Env.Load();
        
        var projectId = DotNetEnv.Env.GetString("GOOGLE_CLOUD_PROJECT");

        // Use this if the microservice won't be utlizing scaling to zero. 
        // As it scales and perform better and support more advanced features
        // services.AddSingleton<PublisherClient>(serviceProvider =>
        // {
        //     Console.WriteLine("\nPublisherClient Created!");
        //     // Here you should initialize your PublisherClient with the settings you need.
        //     return PublisherClient.Create(topicName);
        // });
        
        services.AddSingleton<PublisherServiceApiClient>(serviceProvider => {
            //Console.WriteLine("\nPublisherServiceApiClient Created!");     

            var client = PublisherServiceApiClient.Create();

            try {
                // Try to get a non-existent topic.
                var topicName = new TopicName(projectId, "non-existent-topic");
                var response = client.GetTopic(topicName);
            } catch (RpcException e) when (e.Status.StatusCode == StatusCode.NotFound) {
                // If we get a "not found" error, it means we were able to connect to the server.
                Console.WriteLine("\nYou succesfully connected to Google Pub/Sub!");
            } catch (Exception e) {
                // If we get any other exception, it might be a connection error.
                Console.WriteLine($"\nFailed to connect to Pub/Sub server: {e.Message}");
            }

            return client;
        });
        
        services.AddSingleton(serviceProvider => {
            var projectId = DotNetEnv.Env.GetString("GOOGLE_CLOUD_PROJECT");
            var publisherService = serviceProvider.GetRequiredService<PublisherServiceApiClient>();
            return new PubSubService(publisherService, projectId);
        });

        return services;
    }
     
    public static IServiceCollection AddSubscriberServices(this IServiceCollection services)
    {
        
        DotNetEnv.Env.Load();
        
        var projectId = DotNetEnv.Env.GetString("GOOGLE_CLOUD_PROJECT");
        
        services.AddSingleton<SubscriberServiceApiClient>(serviceProvider => {
            Console.WriteLine("\nSubscriberServiceApiClient Created!");     

            var client = SubscriberServiceApiClient.Create();

            return client;
        });
        
        return services;
    }

}