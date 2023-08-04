using x_endpoints.Persistence.MongoDB;
using x_endpoints.DataSeeder;
using x_endpoints.Persistence.Google_PubSub;
using x_endpoints;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

DotNetEnv.Env.Load();

var serviceName = DotNetEnv.Env.GetString("SERVICE_NAME");

Console.WriteLine($"\n{serviceName}");

Console.WriteLine("###################################");

var enviroment = DotNetEnv.Env.GetString("ENVIRONMENT");

// Add / Disable MongoDB
builder.Services.AddMongoDBServices();
// Add / Disable Publisher
builder.Services.AddPublisherServices();
// Add / Disable Subscriber 
//builder.Services.AddSubscriberServices();

// Hosting to make sure it dependencies connect on Program startup
builder.Services.AddHostedService<MongoDbStartupService>();
builder.Services.AddHostedService<PubSubStartupService>();

// Add this after all project dependencies to register all the services.
builder.Services.AddApplicationServices(); 

// Testing Google Pub / Sub
// var projectId = DotNetEnv.Env.GetString("GOOGLE_CLOUD_PROJECT");
// var topicId = "test-topic2";
// var subscriptionId = topicId;

// // First create a topic.
// PublisherServiceApiClient publisherService = await PublisherServiceApiClient.CreateAsync();
// TopicName topicName = new TopicName(projectId, topicId);
// publisherService.CreateTopic(topicName);

// // Subscribe to the topic.
// SubscriberServiceApiClient subscriberService = await SubscriberServiceApiClient.CreateAsync();
// SubscriptionName subscriptionName = new SubscriptionName(projectId, subscriptionId);
// subscriberService.CreateSubscription(subscriptionName, topicName, pushConfig: null, ackDeadlineSeconds: 60);

// // Publish a message to the topic using PublisherClient.
// PublisherClient publisher = await PublisherClient.CreateAsync(topicName);

// // PublishAsync() has various overloads. Here we're using the string overload.
// string messageId = await publisher.PublishAsync("Hello, Pubsub");

// // PublisherClient instance should be shutdown after use.
// // The TimeSpan specifies for how long to attempt to publish locally queued messages.
// await publisher.ShutdownAsync(TimeSpan.FromSeconds(15));

// // Pull messages from the subscription using SubscriberClient.
// SubscriberClient subscriber = await SubscriberClient.CreateAsync(subscriptionName);
// List<PubsubMessage> receivedMessages = new List<PubsubMessage>();
// // Start the subscriber listening for messages.
// await subscriber.StartAsync((msg, cancellationToken) =>
// {
//     receivedMessages.Add(msg);
//     Console.WriteLine($"Received message {msg.MessageId} published at {msg.PublishTime.ToDateTime()}");
//     Console.WriteLine($"Text: '{msg.Data.ToStringUtf8()}'");
//     // Stop this subscriber after one message is received.
//     // This is non-blocking, and the returned Task may be awaited.
//     subscriber.StopAsync(TimeSpan.FromSeconds(15));
//     // Return Reply.Ack to indicate this message has been handled.
//     return Task.FromResult(SubscriberClient.Reply.Ack);
// });

// // Tidy up by deleting the subscription and the topic.
// subscriberService.DeleteSubscription(subscriptionName);
// publisherService.DeleteTopic(topicName);

//Adding the Controllers
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", builder =>
    {
        builder
            .AllowAnyOrigin() // or specify the allowed origins
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

if (enviroment.Equals("Development")) {

    // Insert initial data into the "Products" collection
    DataSeeder.SeedData(app.Services);
    Console.WriteLine("\nSeeding Database due to ENV: Development...\n");

}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("MyCorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();