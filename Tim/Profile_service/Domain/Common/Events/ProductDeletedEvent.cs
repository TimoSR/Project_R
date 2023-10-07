using Domain.Common.Events._Interfaces;
using Domain.DomainEvents._Attributes;
using ProtoBuf;

namespace Domain.DomainEvents;

[ProtoContract]
[TopicName("ProductDeletedTopic")]
public class ProductDeletedEvent : IPubEvent
{
    [ProtoMember(1)]
    public string test { get; set; }
}