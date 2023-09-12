using x_endpoints.Helpers;
using x_endpoints.Persistence.MongoDB;
using x_endpoints.Persistence.Google_PubSub;
using x_endpoints.Persistence.StartUp;
using x_endpoints.Registration.DataSeeder;
using x_endpoints.Registration.GraphQL;
using x_endpoints.Registration.Repositories;
using x_endpoints.Registration.Services;
using x_endpoints.Registration.Tools;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

var hostUrl = DotNetEnv.Env.GetString("HOST_URL");
var serviceName = DotNetEnv.Env.GetString("SERVICE_NAME");
var projectId = DotNetEnv.Env.GetString("GOOGLE_CLOUD_PROJECT");
var environment = DotNetEnv.Env.GetString("ENVIRONMENT");
var mongoConnectionString = DotNetEnv.Env.GetString("MONGODB_CONNECTION_STRING");
var redisConnectionString = DotNetEnv.Env.GetString("REDIS_CONNECTION_STRING");
var envVars = Environment.GetEnvironmentVariables();

var config = new Configuration()
{
    HostUrl = hostUrl,
    ProjectId = projectId,
    ServiceName = serviceName,
    Environment = environment,
    MongoConnectionString = mongoConnectionString,
    RedisConnectionString = redisConnectionString,
    EnvironmentVariables = envVars
};

builder.Services.AddSingleton(config);

Console.WriteLine($"\n{serviceName}");

// Custom Tools written tools to simplify development
builder.Services.AddApplicationTools();

// Add / Disable GraphQL (MapGraphQL should be out-commented too)
builder.Services.AddGraphQLServices(); 
// Add / Disable MongoDB
builder.Services.AddMongoDBServices(config);
// Add / Disable Publisher
builder.Services.AddPublisherServices(config);
// Add / Disable Subscriber 
builder.Services.AddSubscriberServices();
// Add / Disable Redis
//builder.Services.AddRedisServices(config);

// Hosting to make sure it dependencies connect on Program startup
builder.Services.AddHostedService<StartExternalConnections>();

// Adding Dependencies to Service Dependency Container
builder.Services.AddSingleton<IServiceDependencies, ServiceDependencies>();

// Adding Database Repositories
builder.Services.AddApplicationRepositories();

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

if (environment.Equals("Development")) {

    // Insert initial data into the MongoDB collections

    var seederType = typeof(IDataSeeder);
    var seeders = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(s => s.GetTypes())
        .Where(p => seederType.IsAssignableFrom(p) && !p.IsInterface)
        .ToList();

    foreach(var seeder in seeders)
    {
        var instance = Activator.CreateInstance(seeder) as IDataSeeder;
        instance?.SeedData(app.Services);
    }
    
    Console.WriteLine("\n###################################");
    Console.WriteLine("\nSeeding Database due to ENV: Development...");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable this for Https only
//app.UseHttpsRedirection();

app.UseCors("MyCorsPolicy");

app.UseAuthorization();

app.MapControllers();

// Websockets is required to enable subscriptions with GraphQL
app.UseWebSockets();

app.MapGraphQL();

app.Run();