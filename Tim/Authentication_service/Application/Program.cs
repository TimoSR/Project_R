using System.Reflection;
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
using Infrastructure.Registrations.Repositories;
using Infrastructure.Registrations.Utilities;
using Infrastructure.Swagger;
using Infrastructure.Swagger.Attributes;
using Infrastructure.Utilities;
using Infrastructure.Utilities.Containers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
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
            JwtIssuer = serviceName,
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
        //builder.Services.AddRedisServices(config);

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
        builder.Services.AddSwaggerGen(c =>
        {
            var documentConfigs = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsSubclassOf(typeof(ControllerBase)))
                .SelectMany(t =>
                    t.GetCustomAttributes<SwaggerDocAttribute>().Select(d => new { DocName = d.DocName, ControllerType = t })
                        .Concat(
                            t.GetCustomAttributes<ApiVersionAttribute>().Select(v => new { DocName = v.Version, ControllerType = t })
                        )
                )
                .Select(item => new
                {
                    DocName = item.DocName,
                    ApiVersion = item.ControllerType.GetCustomAttribute<ApiVersionAttribute>()?.Version
                })
                .Distinct();

            Dictionary<string, List<string>> docNameToVersionMap = new Dictionary<string, List<string>>();

            foreach (var config in documentConfigs)
            {
                var docKey = $"{config.DocName} {config.ApiVersion}";
                c.SwaggerDoc(docKey, new OpenApiInfo { Title = config.DocName, Version = config.ApiVersion });
                if (!docNameToVersionMap.ContainsKey(config.DocName))
                {
                    docNameToVersionMap[config.DocName] = new List<string>();
                }
                docNameToVersionMap[config.DocName].Add(config.ApiVersion);
            }

            c.DocInclusionPredicate((docName, apiDesc) =>
            {
                var controllerActionDescriptor = apiDesc.ActionDescriptor as ControllerActionDescriptor;
                if (controllerActionDescriptor != null)
                {
                    var swaggerDocAttr = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttribute<SwaggerDocAttribute>();
                    var apiVersionAttr = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttribute<ApiVersionAttribute>();

                    if (swaggerDocAttr != null && apiVersionAttr != null)
                    {
                        var expectedDocName = $"{swaggerDocAttr.DocName} {apiVersionAttr.Version}";
                        return docName.Equals(expectedDocName, StringComparison.OrdinalIgnoreCase);
                    }
                }
                return false;
            });
        
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new List<string>()
                }
            });
        });

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
        
        // The standard way of implementing jwt auth in dotnet
        // Tried to implement my own method of handling it.
        // builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //     .AddJwtBearer(options =>
        //     {
        //         options.TokenValidationParameters = new TokenValidationParameters
        //         {
        //             ValidateIssuer = true,
        //             ValidateAudience = true,
        //             ValidateLifetime = true,
        //             ValidateIssuerSigningKey = true,
        //             ValidIssuer = config.JwtIssuer,
        //             ValidAudience = config.JwtAudience,
        //             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.JwtKey))
        //         };
        //     });
        
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
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                var documentConfigs = new List<DocumentConfig>();

                var controllerTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .Where(t => t.IsSubclassOf(typeof(ControllerBase)));

                foreach (var controllerType in controllerTypes)
                {
                    var swaggerDocAttribute = controllerType.GetCustomAttribute<SwaggerDocAttribute>();
                    var apiVersionAttribute = controllerType.GetCustomAttribute<ApiVersionAttribute>();

                    if (swaggerDocAttribute != null && apiVersionAttribute != null)
                    {
                        var apiVersion = apiVersionAttribute.Version; // Updated this line
                        documentConfigs.Add(new DocumentConfig
                        {
                            DocName = swaggerDocAttribute.DocName,
                            ApiVersion = apiVersion
                        });
                    }
                }

                // Sort the document configs by DocName and then by ApiVersion
                var sortedDocumentConfigs = documentConfigs
                    .OrderBy(dc => dc.DocName)
                    .ThenBy(dc => dc.ApiVersion)
                    .ToList();

                foreach (var config in sortedDocumentConfigs)
                {
                    var endpointName = $"{config.DocName} {config.ApiVersion}";
                    c.SwaggerEndpoint($"/swagger/{config.DocName} {config.ApiVersion}/swagger.json", endpointName); // Updated this line
                }
            });
        }

        // Enable this for Https only
        //app.UseHttpsRedirection();
        
        // Controller Middlewares
        app.UseCors("MyCorsPolicy");
        app.UseIpRateLimiting();
        // Jwt Authentication
        app.UseMiddleware<JwtMiddleware>();
        //app.UseAuthentication(); // This is the disabled auth.
        app.UseAuthorization();

        app.MapControllers();

        // Websockets is required to enable subscriptions with GraphQL
        app.UseWebSockets();

        app.MapGraphQL();

        await app.RunAsync();
    }
}

