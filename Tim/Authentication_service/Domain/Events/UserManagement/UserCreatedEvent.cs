using _SharedKernel.Patterns.RegistrationHooks.Events._Attributes;
using _SharedKernel.Patterns.RegistrationHooks.Events._Interfaces;
using ProtoBuf;

namespace Domain.Events.UserManagement;

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