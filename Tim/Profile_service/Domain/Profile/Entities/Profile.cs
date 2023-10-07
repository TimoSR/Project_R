using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Profile.Entities;

public class Profile
{
    [BsonId]
    [BsonElement("UserId")]
    public string UserID { get; set; }
        
    [BsonElement("DisplayName")]
    public string DisplayName { get; set; }

    [BsonElement("CreationDate")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreationDate { get; } = DateTime.UtcNow;

    [BsonElement("LastLoginDate")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime? LastLoginDate { get; set; }
}