

using Newtonsoft.Json;
using x_endpoints.Tools.Serializers.Types;

namespace x_endpoints.Tools.Serializers;

public class JsonSerializer<TPayload> : ISerializer<TPayload>
{
    public string Serialize(TPayload content)
    {
        try
        {
            var message = BuildMessage(content);
            return ConvertToFormat(message);
        }
        catch (JsonException ex)
        {
            // Handle JSON-specific errors.
            // For example, log the error or provide a specific message for JSON errors.
            throw new InvalidOperationException($"Failed to serialize content of type {typeof(TPayload).Name} due to JSON error.", ex);
        }
        catch (Exception ex) // This will catch any other exception
        {
            // Handle generic errors.
            // You might want to log the error or take some other action.
            throw new InvalidOperationException($"An unexpected error occurred while serializing content of type {typeof(TPayload).Name}.", ex);
        }
    }

    private JsonMessage<TPayload> BuildMessage(TPayload content)
    {
        return new JsonMessage<TPayload> { Content = content };
    }

    private string ConvertToFormat(JsonMessage<TPayload> message)
    {
        return JsonConvert.SerializeObject(message);
    }
} 