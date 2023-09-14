using ProtoBuf;
using x_App.DomainEvents._Interfaces;
using x_App.Infrastructure.Helpers.Attributes;

namespace x_App.DomainEvents;

[ProtoContract]
[TopicName("ProductCreatedTopic")]
public class ProductCreatedEvent : IPubEvent
{
    [ProtoMember(1)]
    public string test { get; set; }
}