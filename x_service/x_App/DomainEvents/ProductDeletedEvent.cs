using ProtoBuf;
using x_endpoints.DomainEvents._Interfaces;
using x_endpoints.Infrastructure.Helpers.Attributes;

namespace x_endpoints.DomainEvents;

[ProtoContract]
[TopicName("ProductDeletedTopic")]
public class ProductDeletedEvent : IPubEvent
{
    [ProtoMember(1)]
    public string test { get; set; }
}