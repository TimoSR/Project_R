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

        // Use this if the microservice won't be utlizing scaling to zero. 
        // As it scales and perform better and support more advanced features
        // services.AddSingleton<PublisherClient>(serviceProvider =>
        // {
        //     Console.WriteLine("\nPublisherClient Created!");
        //     // Here you should initialize your PublisherClient with the settings you need.
        //     return PublisherClient.Create(topicName);
        // });
        
        services.AddSingleton<PublisherServiceApiClient>(serviceProvider => {
            Console.WriteLine("\nPublisherServiceApiClient Created!");
            return PublisherServiceApiClient.Create();
        });
        
        services.AddSingleton(serviceProvider => {
            var projectId = DotNetEnv.Env.GetString("GOOGLE_CLOUD_PROJECT");
            var publisherService = serviceProvider.GetRequiredService<PublisherServiceApiClient>();
            return new PubSubService(publisherService, projectId);
        });

        return services;
    }
}