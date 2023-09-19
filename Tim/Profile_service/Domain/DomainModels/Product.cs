using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Domain.DomainModels;

[ProtoContract]
public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [ProtoMember(1)]
    public string Id { get; set; }

    [BsonElement("Name")]
    [ProtoMember(2)]
    public string Name { get; set; }

    [BsonElement("Description")]
    [ProtoMember(3)]
    public string Description { get; set; }

    [BsonElement("Price")]
    [ProtoMember(4)]
    public double Price { get; set; }

    // You can add more properties depending on your needs. Each property represents a field in your Product document.
}