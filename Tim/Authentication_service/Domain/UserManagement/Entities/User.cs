using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.UserManagement.Entities;

public class User
{
    public const int MinPasswordLength = 6;
    
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    private string _email;
    [BsonElement("Email")]
    public string Email 
    { 
        get => _email; 
        set => _email = value?.ToLowerInvariant(); 
    }

    [BsonElement("UserName")]
    public string UserName { get; set; }
}