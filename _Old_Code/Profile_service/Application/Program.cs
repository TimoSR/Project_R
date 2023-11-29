using System.Text;
using Application.Registrations.DataSeeder;
using Application.Registrations.Events;
using Application.Registrations.GraphQL;
using Application.Registrations.Services;
using Application.Startup;
using AspNetCoreRateLimit;
using Infrastructure.Middleware;
using Infrastructure.Persistence.Google_PubSub;
using Infrastructure.Persistence.MongoDB;
using Infrastructure.Persistence.Redis;
using Infrastructure.Registrations.Repositories;
using Infrastructure.Registrations.Utilities;
using Infrastructure.Swagger;
using Infrastructure.Utilities;
using Infrastructure.Utilities.Containers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using IConfiguration = Infrastructure.Utilities._Interfaces.IConfiguration;

namespace Application;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        DotNetEnv.Env.Load();

        var hostUrl = DotNetEnv.Env.GetString("HOST_URL");
        var serviceName = DotNetEnv.Env.GetString("SERVICE_NAME");
        var projectId = DotNetEnv.Env.GetString("GOOGLE_CLOUD_PROJECT");
        var environment = DotNetEnv.Env.GetString("ENVIRONMENT");
        var mongoConnectionString = DotNetEnv.Env.GetString("MONGODB_CONNECTION_STRING");
        var redisConnectionString = DotNetEnv.Env.GetString("REDIS_CONNECTION_STRING");
        var jwtKey = DotNetEnv.Env.GetString("JWT_KEY");
        var jwtIssuer = DotNetEnv.Env.GetString("JWT_ISSUER");
        var jwtAudience = DotNetEnv.Env.GetString("JWT_AUDIENCE");
        var envVars = Environment.GetEnvironmentVariables();

        IConfiguration config = new Configuration()
        {
            HostUrl = hostUrl,
            ProjectId = projectId,
            ServiceName = serviceName,
            Environment = environment,
            MongoConnectionString = mongoConnectionString,
            RedisConnectionString = redisConnectionString,
            EnvironmentVariables = envVars,
            JwtKey = jwtKey,
            JwtIssuer = jwtIssuer,
            JwtAudience = jwtAudience
        };

        builder.Services.AddSingleton(config);

        Console.WriteLine($"\n{serviceName}");

        // Custom Tools written tools to simplify development
        builder.Services.RegisterUtilityServices();

        // Add / Disable GraphQL (MapGraphQL should be out-commented too)
        builder.Services.AddGraphQlServices(); 
        // Add / Disable MongoDB
        builder.Services.AddMongoDbServices(config);
        // Add / Disable Publisher
        builder.Services.AddPublisherClient(config);
        // Add / Disable Subscriber 
        builder.Services.AddSubscriberClient();
        // Add / Disable Redis
        builder.Services.AddRedisServices(config);

        // Adding All Pub & Sub Events with reflection
        builder.Services.AddSingleton<SubTopicsRegister>();
        builder.Services.AddSingleton<PubTopicsRegister>();

        // Hosting to make sure it dependencies connect on Program startup
        builder.Services.AddHostedService<StartExternalConnections>();

        // Adding Dependencies to Service Dependency Container
        builder.Services.AddScoped<IServiceDependencies, ServiceDependencies>();

        // Adding Database Repositories
        builder.Services.AddApplicationRepositories();

        // Add this after all project dependencies to register all the services.
        builder.Services.AddApplicationServices();

        //Adding the Controllers
        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerServices();

        // Cors Access Rules
        
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
        
        // Default JWT Authentication
        
        // builder.Services.AddAuthentication(options =>
        // {
        //     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        // }).AddJwtBearer(options =>
        // {
        //     options.RequireHttpsMetadata = false;
        //     options.SaveToken = true;
        //     options.TokenValidationParameters = new TokenValidationParameters
        //     {
        //         ValidateIssuer = true,
        //         ValidateAudience = true,
        //         ValidateLifetime = true,
        //         ValidateIssuerSigningKey = true,
        //         ValidIssuer = config.JwtIssuer,
        //         ValidAudience = config.JwtAudience,
        //         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.JwtKey))
        //     };
        // });
        
        
        // This is just a test for the future
        // Adding Roles to the Authentication
        
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("User", policy => policy.RequireRole("User"));
            options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
        });
        
        // Add memory cache services
        builder.Services.AddMemoryCache();
        builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        
        // Adding Rate Limiting 
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
        builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));
        
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
            app.GenerateSwaggerDocs();
        }

        // Enable this for Https only
        //app.UseHttpsRedirection();
        
        // Controller Middlewares
        app.UseCors("MyCorsPolicy");
        app.UseIpRateLimiting();
        
        // Learning about middleware 
        // A test of implementing my own jwt auth
       app.UseMiddleware<JwtValidationMiddleware>();
        
        //app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        // Websockets is required to enable subscriptions with GraphQL
        app.UseWebSockets();

        app.MapGraphQL();

        await app.RunAsync();
    }
}