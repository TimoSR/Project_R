using _CommonLibrary.Patterns.RegistrationHooks.Events._Attributes;
using _CommonLibrary.Patterns.RegistrationHooks.Events._Interfaces;
using ProtoBuf;

namespace Domain.UserManagement.Events;

[ProtoContract]
[TopicName("UserCreatedTopic")]
public class UserCreatedEvent : IPubEvent
{
    public string Message => "User Created!";

    [ProtoMember(1)]
    public string Email { get; set; }
    
    [ProtoMember(2)]
    public string Password { get; set; }
    
}