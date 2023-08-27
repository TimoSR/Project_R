using ProtoBuf;

namespace x_endpoints.Tools.Serializers.Types;

[ProtoContract]
public class ProtobufMessage<TPayload> : IMessage<TPayload>
{
    [ProtoMember(1)]
    public TPayload Content { get; set; }
}
