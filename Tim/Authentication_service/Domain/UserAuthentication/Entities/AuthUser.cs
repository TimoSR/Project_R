using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.UserAuthentication.Entities;

public class AuthUser
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)] 
    public string Id { get; set; }

    private string _email;
    [BsonElement("Email")]
    public string Email { get; set; }

    [BsonElement("HashedPassword")] 
    public string HashedPassword { get; set; }

    [BsonElement("RefreshToken")] 
    public string? RefreshToken { get; set; }

    [BsonElement("EmailValidated")] 
    public bool EmailValidated { get; set; }
}