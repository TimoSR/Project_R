using CommonLibrary.Domain.DomainEvents._Attributes;
using Domain.DomainEvents._Interfaces;
using ProtoBuf;

namespace Domain.DomainEvents;

[ProtoContract]
[TopicName("ProductDeletedTopic")]
public class ProductDeletedEvent : IPubEvent
{
    [ProtoMember(1)]
    public string test { get; set; }
}