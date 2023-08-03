using Google.Cloud.PubSub.V1;
using Grpc.Core;
using MongoDB.Driver;
using MongoDB.Bson;
using x_endpoints.Persistence.MongoDB;
using x_endpoints.Models;
using x_endpoints.Services;
using x_endpoints.DataSeeder;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

var enviroment = DotNetEnv.Env.GetString("ENVIRONMENT");

// Add services to the container.
builder.Services.AddMongoDBServices(builder.Configuration);

// Hosting MangoDB to make sure it connects on Program startup
builder.Services.AddHostedService<MongoDbStartupService>();

// Testing Google Pub / Sub
var googleProjectId = DotNetEnv.Env.GetString("GOOGLE_CLOUD_PROJECT");

// Instantiate client
PublisherServiceApiClient publisherClient = PublisherServiceApiClient.Create();

// Format topic
TopicName topicName = new TopicName(googleProjectId, "test-topic");

try
{
    // Try to get the topic
    Topic topic = publisherClient.GetTopic(topicName);
    Console.WriteLine($"Topic {topicName} already exists.");
}
catch (RpcException e) when (e.Status.StatusCode == StatusCode.NotFound)
{
    // If not found, create the topic
    Topic topic = publisherClient.CreateTopic(topicName);
    Console.WriteLine($"Topic {topicName} created.");
}

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
    Console.WriteLine("\nDatabase is now seeded!\n");

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