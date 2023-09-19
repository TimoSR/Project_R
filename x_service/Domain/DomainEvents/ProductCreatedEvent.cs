using Domain.DomainEvents._Attributes;
using Domain.DomainEvents._Interfaces;
using ProtoBuf;

namespace Domain.DomainEvents;

[ProtoContract]
[TopicName("ProductCreatedTopic")]
public class ProductCreatedEvent : IPubEvent
{
    [ProtoMember(1)]
    public string test { get; set; }
}