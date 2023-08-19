using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace x_endpoints.Interfaces
{
    public interface IItems
    {
        string Id { get; set; }
        string Type { get; set; }
        string Description { get; set; }
        decimal Price { get; set; }
    }
}
