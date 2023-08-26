using Google.Cloud.PubSub.V1;
using Microsoft.AspNetCore.Mvc.Filters;
using x_endpoints.DomainModels;

namespace x_endpoints.Persistence.Google_PubSub.Attribute;

public class PubSubActionFilter
{
    private readonly PubServices _pubServices;

    public PubSubActionFilter(PubServices pubServices)
    {
        _pubServices = pubServices;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var resultContext = await next(); // Execute the actual action

        var attribute = context.ActionDescriptor.EndpointMetadata
            .OfType<PubSubPublishUAttribute>()
            .FirstOrDefault();

        if (attribute != null)
        {   
            var topicID = _pubServices.GenerateTopicID(attribute.ServiceName, attribute.TopicName);
            
            object payload;
            if (string.IsNullOrWhiteSpace(attribute.PayloadArgumentName))
            {
                // If no specific argument name is given, take the first argument (useful for single-argument actions).
                payload = context.ActionArguments.Values.FirstOrDefault();
            }
            else
            {
                context.ActionArguments.TryGetValue(attribute.PayloadArgumentName, out payload);
            }
            
            if (payload is Product product)  // This assumes that the payload should be of type 'Product'.
            {
                await _pubServices.PublishMessageAsync(topicID, $"New product: {product.Name}");
            }
        }
    }
}