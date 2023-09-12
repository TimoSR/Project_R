using System.Collections;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.PubSub.V1;
using x_endpoints.Helpers;
using x_endpoints.Persistence.StartUp;

namespace x_endpoints.Persistence.Google_PubSub;

public class SubTopicsManager
{
    private SubscriberServiceApiClient _subscriberService;
    private readonly string _projectId;
    private readonly string _serviceName;
    private readonly string _environment;
    private readonly IDictionary _environmentVariables;

    public SubTopicsManager(Configuration config, SubscriberServiceApiClient subscriberService)
    {
        _projectId = config.ProjectId;
        _serviceName = config.ServiceName;
        _environment = config.Environment;
        _environmentVariables = config.EnvironmentVariables;
        _subscriberService = subscriberService;
        //IfDevelopment();
        RegisterPullSubscriptions();
        RegisterPushSubscriptions();
        ListAllSubscriptions();        
    }

    private void RegisterPullSubscriptions() 
    {
        // Get all environment variables
        var envVars = _environmentVariables;

        Console.WriteLine("\nRegistering Pull Subscriptions:");

        // Filter environment variables starting with "PULLSUBSCRIBE_"
        foreach (DictionaryEntry variable in envVars)
        {
            string key = variable.Key.ToString();
            if (key.StartsWith("PULL_SUBSCRIBE_"))
            {
                Console.WriteLine($"\n{variable.Key}");
                Console.WriteLine($"{variable.Value}");

                var keyValue = variable.Key.ToString();
                var topicValue = variable.Value.ToString();
                var subscriptionId = $"{topicValue}-{_serviceName}";

                RegisterPullSubscription(subscriptionId, topicValue);
            }
        }
    }  

    private void RegisterPullSubscription(string subscriptionId, string topicValue)
    {
        // Code to create a pull subscription
        // No need to specify a pushEndpoint, just bind the subscription to the topic.
        // If using the Google Cloud Pub/Sub client library, this could be done with:

        var publisher = _subscriberService;
        var topicName = new TopicName(_projectId, topicValue);
        var subscriptionName = new SubscriptionName(_projectId, subscriptionId);

        try
        {
            var subscription = publisher.CreateSubscription(subscriptionName, topicName, pushConfig: null, ackDeadlineSeconds: 60);
            Console.WriteLine($"Pull Subscription {subscriptionName} created.");
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Failed to create pull subscription: {ex.Message}");
        }
    }

    
    private void RegisterPushSubscriptions()
    {
        // Get all environment variables
        var envVars = _environmentVariables;

        Console.WriteLine("\nRegistering Push Subscriptions:");

        // Filter environment variables starting with "PUSH_SUBSCRIBE_"
        foreach (DictionaryEntry variable in envVars)
        {
            string key = variable.Key.ToString();

            if (key.StartsWith("PUSH_SUBSCRIBE_"))
            {
                Console.WriteLine($"\n{variable.Key}");
                Console.WriteLine($"{variable.Value}");

                var keyValue = variable.Key.ToString();
                var topicValue = variable.Value.ToString();
                var subscriptionId = $"{topicValue}-{_serviceName}";

                // Generating the corresponding endpoint environment variable name
                var endpointKey = key.Replace("PUSH_SUBSCRIBE_", "PUSH_ENDPOINT_");

                // If the topic name is PRODUCT_UPDATES, the corresponding endpoint environment variable would be PUSH_ENDPOINT_PRODUCT_UPDATES.
                var pushEndpoint = envVars[endpointKey]?.ToString();

                if (string.IsNullOrEmpty(pushEndpoint))
                {
                    Console.WriteLine($"Warning: Push endpoint for {keyValue} is not defined.");
                    continue;
                }

                RegisterPushSubscription(subscriptionId, topicValue, pushEndpoint);
            }
        }
    }   

    private void RegisterPushSubscription(string subscriptionId, string topicValue, string pushEndpoint)
    {
        SubscriptionName subscriptionName = new SubscriptionName(_projectId, subscriptionId);
        TopicName topicName = new TopicName(_projectId, topicValue);
        PushConfig pushConfig = new PushConfig() { PushEndpoint = pushEndpoint };

        try
        {
            _subscriberService.CreateSubscription(subscriptionName, topicName, pushConfig, ackDeadlineSeconds: 60);
            Console.WriteLine($"\nPush Subscription {subscriptionId} has been created for topic {topicValue}.");
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Failed to create pull subscription: {ex.Message}");
        }
    }

    private void ListAllSubscriptions()
    {
        ProjectName projectName = new ProjectName(_projectId);

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
        var environmentVariables = _environmentVariables;
        var serviceName = _serviceName;

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
        SubscriptionName subscriptionName = new SubscriptionName(_projectId, subscriptionId);
        TopicName topicName = new TopicName(_projectId, topicValue);

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