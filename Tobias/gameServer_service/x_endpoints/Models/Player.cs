using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace x_endpoints.Models;

public class Player
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("Name")]
    public string Name { get; set; }

    [BsonElement("Health")]
    public string Health { get; set; }

    [BsonElement("Level")]
    public string Level {get; set;}
    // Methods
    public void TakeDamage(int damageAmount)
    {
        Health -= damageAmount;
        if (Health <= 0)
        {
            Console.WriteLine($"{Name} has been defeated!");
        }
    }

    public void Heal(int healAmount)
    {
        Health += healAmount;
    }

    public void LevelUp()
    {
        Level++;
        Console.WriteLine($"{Name} leveled up to level {Level}");
    }


    // You can add more properties depending on your needs. Each property represents a field in your Product document.
}