using System.Collections;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.PubSub.V1;
using x_endpoints.Persistence.StartUp;

namespace x_endpoints.Persistence.Google_PubSub;

public class SubTopicsManager
{
    private SubscriberServiceApiClient _subscriberService;
    private readonly string _projectID;
    private readonly string _serviceName;
    private readonly string _environment;

    public SubTopicsManager(Configuration config, SubscriberServiceApiClient subscriberService)
    {
        
        _projectID = config.ProjectId;
        _serviceName = config.ServiceName;
        _environment = config.Environment;
        _subscriberService = subscriberService;
        //IfDevelopment();
        RegisterSubscriptions();
        ListAllSubscriptions();        
    }

    private void IfDevelopment()
    {
        if (_environment == "Development")
        {
            Console.WriteLine("\nDeleting Subscriptions due to ENV: Development...");

            // Get all environment variables
            var envVars = Environment.GetEnvironmentVariables();

            foreach (var key in envVars.Keys)
            {
                // Check if the environment variable starts with 'SUBSCRIBE_'
                if (key.ToString().StartsWith("SUBSCRIBE_"))
                {
                    // Get the subscription name
                    var subscriptionName = $"{envVars[key].ToString()}-{_serviceName}";
                
                    // Delete the subscription
                    var existingSubscription = _subscriberService.GetSubscription(new SubscriptionName(_projectID, subscriptionName));
                    if (existingSubscription != null)
                    {
                        _subscriberService.DeleteSubscription(existingSubscription.SubscriptionName);
                    }
                }
            }
        }
    }

    private void RegisterSubscriptions()
    {
        // Get all environment variables
        var environmentVariables = Environment.GetEnvironmentVariables();

        Console.WriteLine("\nRegistering Subscriptions:");

        // Filter environment variables starting with "SUBSCRIBE_"
        foreach (DictionaryEntry variable in environmentVariables)
        {
            string key = variable.Key.ToString();
            if (key.StartsWith("SUBSCRIBE_"))
            {
                if (key.StartsWith("SUBSCRIBE_"))
                {
                    Console.WriteLine($"\n{variable.Key}");
                    Console.WriteLine($"{variable.Value}");

                    var keyValue = variable.Key.ToString();
                    var topicValue = variable.Value.ToString();
                    var subscriptionId = $"{topicValue}-{_serviceName}";
                    var endpoint = $"{keyValue.ToUpper().Replace("SUBSCRIBE_", "ENDPOINT_")}";

                    // If the topic name is order-updates, the corresponding endpoint environment variable would be ENDPOINT_ORDER_UPDATES.
                    var pushEndpoint = DotNetEnv.Env.GetString(endpoint);

                    RegisterPushSubscription(subscriptionId, topicValue, pushEndpoint);
                }
            }
        }
    }   

    private void RegisterPushSubscription(string subscriptionId, string topicValue, string pushEndpoint)
    {
        SubscriptionName subscriptionName = new SubscriptionName(_projectID, subscriptionId);
        TopicName topicName = new TopicName(_projectID, topicValue);
        PushConfig pushConfig = new PushConfig() { PushEndpoint = pushEndpoint };

        try
        {
            // Check if the subscription already exists
            var existingSubscription = _subscriberService.GetSubscription(subscriptionName);
            Console.WriteLine($"Subscription {subscriptionId} already exists.");
        }
        catch (Grpc.Core.RpcException e) when (e.Status.StatusCode == Grpc.Core.StatusCode.NotFound)
        {
            // If the subscription does not exist, create a new one
            _subscriberService.CreateSubscription(subscriptionName, topicName, pushConfig, ackDeadlineSeconds: 60);
            //Console.WriteLine($"\nSubscription {subscriptionId} has been created for topic {topicValue}.");
        }
    }

    private void ListAllSubscriptions()
    {
        ProjectName projectName = new ProjectName(_projectID);

        var allSubscriptions = _subscriberService.ListSubscriptions(projectName);

        Console.WriteLine("\nSubscriptions in PubSub:\n");

        foreach (var subscription in allSubscriptions)
        {
            Console.WriteLine($"Subscription: {subscription.SubscriptionName}");
        }
    }
    private async void RegisterSubscriptionsAsync()
    {
        // Get all environment variables
        var environmentVariables = Environment.GetEnvironmentVariables();
        var serviceName = DotNetEnv.Env.GetString("SERVICE_NAME");

        Console.WriteLine("\nRegistering Subscriptions:");

        // Filter environment variables starting with "SUBSCRIBE_"
        foreach (DictionaryEntry variable in environmentVariables)
        {
            string key = variable.Key.ToString();
            if (key.StartsWith("SUBSCRIBE_"))
            {
                Console.WriteLine($"\n{variable.Key}");
                Console.WriteLine($"{variable.Value}");
                var topicValue = variable.Value.ToString();
                var subscriptionId = $"{topicValue}-{serviceName}";
                await RegisterSubscriptionAsync(subscriptionId, topicValue);
            }
        }
    }

    private async Task RegisterSubscriptionAsync(string subscriptionId, string topicValue)
    {
        SubscriptionName subscriptionName = new SubscriptionName(_projectID, subscriptionId);
        TopicName topicName = new TopicName(_projectID, topicValue);

        try
        {
            // Check if the subscription already exists
            var existingSubscription = await _subscriberService.GetSubscriptionAsync(subscriptionName);
            Console.WriteLine($"Subscription {subscriptionId} already exists.");
        }
        catch (Grpc.Core.RpcException e) when (e.Status.StatusCode == Grpc.Core.StatusCode.NotFound)
        {
            // If the subscription does not exist, create a new one
            await _subscriberService.CreateSubscriptionAsync(subscriptionName, topicName, pushConfig: null, ackDeadlineSeconds: 60);
            Console.WriteLine($"\nSubscription {subscriptionId} has been created for topic {topicValue}.");
        }
    }
}