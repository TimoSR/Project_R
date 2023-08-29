using Newtonsoft.Json;

namespace x_endpoints.Tools.Serializers;

public class JsonSerializer<TPayload> : ISerializer<TPayload>
{
    public string Serialize(TPayload content)
    {
        try
        {
            Console.WriteLine("\nCreated Json Message!");
            string encodedString = ConvertToFormat(content);
            Console.WriteLine(encodedString);
            return encodedString;
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

    private string ConvertToFormat(TPayload message)
    {
        return JsonConvert.SerializeObject(message);
    }
} 