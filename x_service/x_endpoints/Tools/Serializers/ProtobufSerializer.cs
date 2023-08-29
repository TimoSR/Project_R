using ProtoBuf;
using System;

namespace x_endpoints.Tools.Serializers;

public class ProtobufSerializer<TPayload> : ISerializer<TPayload>
{
    
    // It only works with models that uses Protobuf-Net Attributes!!!!
    
    public string Serialize(TPayload content)
    {
        try
        {
            Console.WriteLine("\nCreated Protobuf Message:");
        
            string encodedString = ConvertToProtobuf(content);
 
            Console.WriteLine(encodedString); // This will print the Base64 string to the console
        
            return encodedString;
        }
        catch (ProtoException ex)
        {
            // Handle protobuf-specific errors.
            throw new InvalidOperationException($"Failed to serialize content of type {typeof(TPayload).Name} due to protobuf error.", ex);
        }
        catch (Exception ex) // This will catch any other exception
        {
            // Handle generic errors.
            throw new InvalidOperationException($"An unexpected error occurred while serializing content of type {typeof(TPayload).Name}.", ex);
        }
    }
    
    public TPayload Deserialize(string content)
    {
        try
        {
            Console.WriteLine("\nDeserializing Protobuf Message:");
                
            TPayload payload = ConvertFromProtobuf(content);
                
            Console.WriteLine($"Deserialized object of type {typeof(TPayload).Name} successfully!");
                
            return payload;
        }
        catch (ProtoException ex)
        {
            // Handle protobuf-specific errors.
            throw new InvalidOperationException($"Failed to deserialize content to type {typeof(TPayload).Name} due to protobuf error.", ex);
        }
        catch (Exception ex) // This will catch any other exception
        {
            // Handle generic errors.
            throw new InvalidOperationException($"An unexpected error occurred while deserializing content to type {typeof(TPayload).Name}.", ex);
        }
    }

    private string ConvertToProtobuf(TPayload message)
    {
        using var ms = new MemoryStream();
        Serializer.Serialize(ms, message);
        byte[] byteArray = ms.ToArray();
        return Convert.ToBase64String(byteArray);
    }
    
    private TPayload ConvertFromProtobuf(string content)
    {
        byte[] byteArray = Convert.FromBase64String(content);
        using var ms = new MemoryStream(byteArray);
        return Serializer.Deserialize<TPayload>(ms);
    }
}  