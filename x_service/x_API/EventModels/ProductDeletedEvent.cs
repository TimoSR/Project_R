using ProtoBuf;
using x_endpoints.EventModels._Interfaces;
using x_endpoints.Helpers.Attributes;

namespace x_endpoints.EventModels;

[ProtoContract]
[TopicName("ProductDeletedTopic")]
public class ProductDeletedEvent : IPubEvent
{
    [ProtoMember(1)]
    public string test { get; set; }
}