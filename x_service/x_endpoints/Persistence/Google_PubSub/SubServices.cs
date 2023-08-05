using System.Collections;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.PubSub.V1;

namespace x_endpoints.Persistence.Google_PubSub;

public class SubServices
{
    private SubscriberServiceApiClient _subscriberService;
    private string _projectID;

    public SubServices(SubscriberServiceApiClient subscriberService, string projectID)
    {
        _subscriberService = subscriberService;
        _projectID = projectID;
        IfDevelopment();
        RegisterSubscriptions();
        ListAllSubscriptions();        
    }

    private void IfDevelopment()
    {
        if (Environment.GetEnvironmentVariable("ENVIRONMENT") == "Development")
        {
            Console.WriteLine("\nDeleting Subscriptions due to ENV: Development...");

            // Delete all existing subscriptions in development environment
            var projectName = new ProjectName(_projectID);
            var existingSubscriptions = _subscriberService.ListSubscriptions(projectName);
            foreach (var existingSubscription in existingSubscriptions)
            {
                _subscriberService.DeleteSubscription(existingSubscription.SubscriptionName);
            }
        }
    }

    private void RegisterSubscriptions()
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
                RegisterSubscription(subscriptionId, topicValue);
            }
        }
    }   

    private void RegisterSubscription(string subscriptionId, string topicValue)
    {
        SubscriptionName subscriptionName = new SubscriptionName(_projectID, subscriptionId);
        TopicName topicName = new TopicName(_projectID, topicValue);

        try
        {
            // Check if the subscription already exists
            var existingSubscription = _subscriberService.GetSubscription(subscriptionName);
            Console.WriteLine($"Subscription {subscriptionId} already exists.");
        }
        catch (Grpc.Core.RpcException e) when (e.Status.StatusCode == Grpc.Core.StatusCode.NotFound)
        {
            // If the subscription does not exist, create a new one
            _subscriberService.CreateSubscription(subscriptionName, topicName, pushConfig: null, ackDeadlineSeconds: 60);
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