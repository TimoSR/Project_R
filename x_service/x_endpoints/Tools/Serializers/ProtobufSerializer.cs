using ProtoBuf;
using x_endpoints.Tools.Serializers.Types;

namespace x_endpoints.Tools.Serializers;

public class ProtobufSerializer<TPayload> : ISerializer<ProtobufMessage<TPayload>, TPayload>, IApplicationTool
{
    public string Serialize(TPayload content)
    {
        try
        {
            var message = BuildMessage(content);
            return ConvertToFormat(message);
        }
        catch (ProtoBuf.ProtoException ex)
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
        return Convert.ToBase64String(ms.ToArray());
    }
}  