using Google.Cloud.PubSub.V1;
using Grpc.Core;
using MongoDB.Driver;
using MongoDB.Bson;
using x_endpoints.Persistence.MongoDB;
using x_endpoints.Models;
using x_endpoints.Services;
using x_endpoints.DataSeeder;
using x_endpoints.Persistence.Google_PubSub;
using x_endpoints;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

var serviceName = DotNetEnv.Env.GetString("SERVICE_NAME");

Console.WriteLine($"\n{serviceName}");

Console.WriteLine("###################################");

var enviroment = DotNetEnv.Env.GetString("ENVIRONMENT");

// Add services to the container.
// Hosting MangoDB to make sure it connects on Program startup
builder.Services.AddMongoDBServices();
builder.Services.AddHostedService<MongoDbStartupService>();

// Add services to the container.
// Hosting PubSub to make sure it connects on Program startup
//builder.Services.AddPubSubServices();
//builder.Services.AddHostedService<PubSubStartupService>();

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