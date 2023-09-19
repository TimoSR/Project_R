using Domain.DomainEvents.EventAttributes;
using ProtoBuf;
using x_Domain.Attributes;

namespace Domain.DomainEvents;

[ProtoContract]
[TopicName("ProductCreatedTopic")]
public class ProductCreatedEvent : IPubEvent
{
    [ProtoMember(1)]
    public string test { get; set; }
}