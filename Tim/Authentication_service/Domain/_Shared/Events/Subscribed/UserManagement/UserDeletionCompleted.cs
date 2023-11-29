using _SharedKernel.Patterns.RegistrationHooks.Events._Attributes;
using ProtoBuf;

namespace Domain._Shared.Events.Subscribed.UserManagement;

[ProtoContract]
[TopicName("UserDeletionCompletedTopic")]
public class UserDeletionCompleted
{
    
    public string Email { get; set; }
    public string Message => $"User {Email} Deletion Completed!";
}