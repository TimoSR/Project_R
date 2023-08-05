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
    }

    private void IfDevelopment()
    {
        if (Environment.GetEnvironmentVariable("ENVIRONMENT") == "Development")
        {
            Console.WriteLine("\nDeleting Subscriptions due to ENV: Development...");

            // Delete all existing subscriptions in development environment
            var projectName = new ProjectName(_projectId);
            var existingSubscriptions = _subscriberService.ListSubscriptions(projectName);
            foreach (var existingSubscription in existingSubscriptions)
            {
                _subscriberService.DeleteSubscription(existingSubscription.SubscriptionName);
            }
        }
    }

    private async void RegisterSubscriptions()
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
                var subscriptionId = $"{variable.Value.ToString()}-{serviceName}";
                await RegisterSubscription(subscriptionId);
            }
        }
    }

    private async Task RegisterSubscription(string subscriptionId)
    {
        var subscriptionName = SubscriptionName.FromProjectSubscription(_projectId, subscriptionId);
        try 
        {
            await _subscriberService.GetSubscriptionAsync(subscriptionName);
        } 
        catch (Grpc.Core.RpcException e) when (e.Status.StatusCode == Grpc.Core.StatusCode.NotFound)
        {
            // Assuming a topic with the same ID exists.
            var topicName = TopicName.FromProjectTopic(_projectId, subscriptionId);
            await _subscriberService.CreateSubscriptionAsync(subscriptionName, topicName, pushConfig: null, ackDeadlineSeconds: 60);
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
}