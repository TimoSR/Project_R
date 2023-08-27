using ProtoBuf;
using System;
using x_endpoints.Tools.Serializers.Types;

namespace x_endpoints.Tools.Serializers;

public class ProtobufSerializer<TPayload> : ISerializer<TPayload>
{
    
    // It only works with models that uses Protobuf-Net Attributes!!!!
    
    public string Serialize(TPayload content)
    {
        try
        {
            var message = BuildMessage(content);
            Console.WriteLine("\nCreated Protobuf Message!");
            Console.WriteLine($"\n{message}");
        
            string encodedString = ConvertToFormat(message);
        
            Console.WriteLine("Encoded Protobuf Message (Base64):");
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

    private ProtobufMessage<TPayload> BuildMessage(TPayload content)
    {
        return new ProtobufMessage<TPayload> { Content = content };
    }

    private string ConvertToFormat(ProtobufMessage<TPayload> message)
    {
        using var ms = new MemoryStream();
        Serializer.Serialize(ms, message);
        byte[] byteArray = ms.ToArray();
        return Convert.ToBase64String(byteArray);
    }
}  