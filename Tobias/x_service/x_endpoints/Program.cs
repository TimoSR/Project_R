using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using Grpc.Core;
using StackExchange.Redis;
using x_endpoints.Persistence.MongoDB;
using x_endpoints.DataSeeder;
using x_endpoints.Persistence.Google_PubSub;
using x_endpoints;
using x_endpoints.Persistence.Redis;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddSubscriberServices();
// Add / Disable Redis
//builder.Services.AddRedisServices();

// Hosting to make sure it dependencies connect on Program startup
builder.Services.AddHostedService<MongoDbStartupService>();
builder.Services.AddHostedService<PubSubStartupService>();
//builder.Services.AddHostedService<RedisStartupService>();

// Add this after all project dependencies to register all the services.
builder.Services.AddApplicationServices();

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
    Console.WriteLine("\n###################################");
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