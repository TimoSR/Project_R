using x_endpoints.DomainModels.Equipments;
using x_endpoints.DomainServices;
using x_endpoints.Enums;

namespace x_endpoints.GameObjectLibrary.Equipment;

public static class Leg
{
    public static async Task SeedData(IServiceProvider serviceProvider)
    {
        var armorService = serviceProvider.GetRequiredService<ArmorService>();

        var legs = new List<Armor>
        {
            new Armor
            {
                Name = "Iron Leggings",
                LevelRequirement = 10,
                ArmorValue = 15,
                Slot = EquipmentSlot.Legs
            },
            new Armor
            {
                Name = "Steel Greaves",
                LevelRequirement = 15,
                ArmorValue = 20,
                Slot = EquipmentSlot.Legs
            },
            new Armor
            {
                Name = "Enchanted Legplates",
                LevelRequirement = 20,
                ArmorValue = 25,
                Slot = EquipmentSlot.Legs
            },
            // Add more leg armor items as needed
        };

        foreach (var leg in legs)
        {
            await armorService.InsertAsync(leg);
        }
    }
    
}