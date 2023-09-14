using ProtoBuf;
using x_Domain.Attributes;

namespace x_Domain.DomainEvents;

[ProtoContract]
[TopicName("ProductDeletedTopic")]
public class ProductDeletedEvent : IPubEvent
{
    [ProtoMember(1)]
    public string test { get; set; }
}