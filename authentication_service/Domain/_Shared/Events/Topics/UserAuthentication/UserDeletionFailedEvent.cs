using _SharedKernel.Patterns.RegistrationHooks.Events._Attributes;
using _SharedKernel.Patterns.RegistrationHooks.Events._Interfaces;
using ProtoBuf;

namespace Domain._Shared.Events.Topics.UserAuthentication;

[ProtoContract]
[TopicName("UserDeletionFailedTopic")]
public class UserDeletionFailedEvent : IPubEvent
{
    public string Message => "User auth data deletion failed";
    
    [ProtoMember(1)]
    public string Email { get; set; }

    [ProtoMember(2)]
    public string Reason { get; set; }
}