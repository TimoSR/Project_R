using x_endpoints.DomainModels.Equipments;
using x_endpoints.DomainServices;

namespace x_endpoints.GameObjectLibrary.Equipment;

public static class Sword
{
    public static async Task SeedData(IServiceProvider serviceProvider)
    {
        var itemService = serviceProvider.GetRequiredService<WeaponService>();

        var swords = new List<Weapon>
        {
            new Weapon
            {
                Name = "Short Sword",
                LevelRequirement = 10,
                AttackValue = 20
            },
            new Weapon
            {
                Name = "Steel Dagger",
                LevelRequirement = 15,
                AttackValue = 18
            },
            new Weapon
            {
                Name = "Rapier",
                LevelRequirement = 20,
                AttackValue = 25
            },
            new Weapon
            {
                Name = "Curved Blade",
                LevelRequirement = 25,
                AttackValue = 22
            },
            new Weapon
            {
                Name = "Enchanted Saber",
                LevelRequirement = 30,
                AttackValue = 30
            }   
        };
        
        foreach (var sword in swords)
        {
            await itemService.InsertAsync(sword);
        }
    }
}


