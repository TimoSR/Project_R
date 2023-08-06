using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using x_endpoints.Models;
using x_endpoints.Services;

namespace x_endpoints.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PubSubController : ControllerBase
{
    private readonly ProductService _productService;

    public PubSubController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpPost("Subscription1")]
    public async Task<IActionResult> HandleSubscription1()
    {
        using (StreamReader reader = new StreamReader(Request.Body))
            {
                var body = await reader.ReadToEndAsync();
                // Log or do something with the message here
                Console.WriteLine($"{body}");
            }

            // Respond with a 200 to acknowledge receipt of the message
            return Ok();
    }

    [HttpPost("InsertProduct")]
    public async Task<IActionResult> ExampleInsertProduct()
    {
        
        var product1 = new Product
        {
            Name = "Product 1",
            Description = "This is product 1",
            Price = 29.99m
        };

        await _productService.InsertProduct(product1);

        return Ok();
    }

    // [HttpPost("InsertProduct")]
    // public async Task<IActionResult> ExampleInsertProduct()
    // {
    //     var projectId = DotNetEnv.Env.GetString("GOOGLE_CLOUD_PROJECT");
    //     var topicId = "x-service-product-updates-v1";
    //     var subscriptionId = "x-service-product-updates-v1-x-service";
    //     string pushEndpoint = "https://0349-185-96-183-231.ngrok-free.app/api/PubSub/Subscription1";

    //     // Create a publisher client
    //     PublisherServiceApiClient publisher = await PublisherServiceApiClient.CreateAsync();
    //     TopicName topicName = new TopicName(projectId, topicId);

    //     // Create the topic if it doesn't exist
    //     try
    //     {
    //         await publisher.GetTopicAsync(topicName);
    //     }
    //     catch (RpcException e) when (e.Status.StatusCode == Grpc.Core.StatusCode.NotFound)
    //     {
    //         await publisher.CreateTopicAsync(topicName);
    //     }

    //     // Create a subscriber client
    //     SubscriberServiceApiClient subscriber = await SubscriberServiceApiClient.CreateAsync();
    //     SubscriptionName subscriptionName = new SubscriptionName(projectId, subscriptionId);

    //     // Create the subscription if it doesn't exist
    //     PushConfig pushConfig = new PushConfig() { PushEndpoint = pushEndpoint };
    //     try
    //     {
    //         await subscriber.GetSubscriptionAsync(subscriptionName);
    //     }
    //     catch (RpcException e) when (e.Status.StatusCode == Grpc.Core.StatusCode.NotFound)
    //     {
    //         await subscriber.CreateSubscriptionAsync(new Subscription()
    //         {
    //             SubscriptionName = subscriptionName,
    //             TopicAsTopicName = topicName,
    //             PushConfig = pushConfig,
    //             AckDeadlineSeconds = 60
    //         });
    //     }

    //     // Publish a message
    //     PubsubMessage message = new PubsubMessage
    //     {
    //         Data = ByteString.CopyFromUtf8("Hello, Pubsub"),
    //         Attributes = { { "description", "Simple text message" } }
    //     };
    //     await publisher.PublishAsync(topicName, new[] { message });

    //     // Run for 5 seconds
    //     await Task.Delay(5000);
        
    //     return Ok();
    // }
}