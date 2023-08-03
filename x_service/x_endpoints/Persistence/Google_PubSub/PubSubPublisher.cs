// using Google.Cloud.PubSub.V1;
// using Grpc.Core;
//
// namespace x_endpoints.Persistence.Google_PubSub;
//
// public static class PubSubPublisher
// {
//     public static async Task<IServiceCollection> AddPubSubPublisher(this IServiceCollection services, IConfiguration configuration)
//     {
//         DotNetEnv.Env.Load();
//         
//         PublisherServiceApiClient publisherService = await PublisherServiceApiClient.CreateAsync();
//         TopicName topicName = new TopicName(projectId, topicId);
//         publisherService.CreateTopic(topicName);
//         
//         services.AddSingleton<PublisherServiceApiClient>(sp =>
//         {
//             // Getting the environment variable
//             var projectName = DotNetEnv.Env.GetString("GOOGLE_PROJECT_NAME");
//
//             // Create the client with the specified settings
//             var publisherClient = PublisherServiceApiClient.Create(clientSettings);
//
//             // Ping test
//             try
//             {
//                 // Format a topic
//                 TopicName topicName = new TopicName(projectName, "ping-test");
//
//                 // Try to get the topic
//                 Topic topic = publisherClient.GetTopic(topicName);
//                 Console.Write("\nStarting Publisher...");
//                 Console.WriteLine("\nPinged your Pub/Sub service. You successfully connected to Google Cloud Pub/Sub! \n");
//             }
//             catch (RpcException e) when (e.Status.StatusCode == StatusCode.NotFound)
//             {
//                 // If not found, just log
//                 Console.WriteLine("\nPing topic not found, but the client is still initialized. \n");
//             }
//             catch (Exception ex)
//             {
//                 Console.WriteLine($"\n{ex}");
//             }
//
//             return publisherClient;
//         });
//
//         
//         return services;
//
//     }
// }