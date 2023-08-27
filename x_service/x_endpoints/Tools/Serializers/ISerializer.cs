using x_endpoints.Tools.Serializers.Types;

namespace x_endpoints.Tools.Serializers;

public interface ISerializer<TMessage, TPayload> where TMessage : IMessage<TPayload>
{
    string Serialize(TPayload content);
}