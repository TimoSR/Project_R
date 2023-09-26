using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.DomainModels
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } // The unique identifier for this user.
        
        [BsonElement("Email")]
        public string Email { get; set; } // The user's email address.
        
        [BsonElement("Password")]
        public string Password { get; set; } // The user's hashed password.
        
        [BsonElement("RefreshToken")]
        public string RefreshToken { get; set; } // The refresh token for this user.
    }
}