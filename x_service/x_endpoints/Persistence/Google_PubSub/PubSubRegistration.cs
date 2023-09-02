using System.Collections;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;

namespace x_endpoints.Persistence.Google_PubSub;

public static class PubSubRegistration {
    
    // public static IServiceCollection AddPubSubServices(this IServiceCollection services, IWebHostEnvironment env)
    // {
    //     
    //     // Get the current environment (e.g., Development, Production)
    //     string environment = DotNetEnv.Env.GetString("ASPNETCORE_ENVIRONMENT");
    //
    //     // If it's development, set the PUBSUB_EMULATOR_HOST variable
    //     if (environment == "Development")
    //     {
    //
    //     }
    // }   

    public static IServiceCollection AddPublisherServices(this IServiceCollection services)
    {
        
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
                Console.WriteLine("\n###################################");
                Console.WriteLine("\nYou successfully connected to PubSub with PublisherApiClient!");
            } catch (Exception e) {
                // If we get any other exception, it might be a connection error.
                Console.WriteLine($"\nFailed to connect to Pub/Sub server: {e.Message}");
            }

            return client;
        });
        
        services.AddSingleton(serviceProvider => {
            var projectId = DotNetEnv.Env.GetString("GOOGLE_CLOUD_PROJECT");
            var publisherService = serviceProvider.GetRequiredService<PublisherServiceApiClient>();
            return new PubTopicsManager(publisherService, projectId);
        });
        
        services.AddSingleton<PubSubEventPublisher>();
            
        return services;
    }
     
    public static IServiceCollection AddSubscriberServices(this IServiceCollection services)
    {

        var projectID = DotNetEnv.Env.GetString("GOOGLE_CLOUD_PROJECT");

        services.AddSingleton<SubscriberServiceApiClient>(serviceProvider =>
        {
            //Console.WriteLine("\nSubscriberServiceApiClient Created!");

            var client = SubscriberServiceApiClient.Create();

            Console.WriteLine("\n###################################");
            Console.WriteLine("\nYou successfully connected to PubSub with SubscriberApiClient!");

            return client;
        });

        services.AddSingleton(serviceProvider =>
        {
            var projectId = DotNetEnv.Env.GetString("GOOGLE_CLOUD_PROJECT");
            var subscriberService = serviceProvider.GetRequiredService<SubscriberServiceApiClient>();
            return new SubTopicsManager(subscriberService, projectId);
        });

        return services;
    }

}