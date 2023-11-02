using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using x_endpoints.DomainModels.Equipments;
using x_endpoints.DomainServices;
using x_endpoints.Enums;

namespace x_endpoints.GameObjectLibrary.Equipment
{
    public static class Pickaxes
    {
        public static List<Pickaxe> _pickaxes = new List<Pickaxe>
        {
            new Pickaxe
            {
                Name = "Wooden pickaxe",
                RequiredLevel = 1,
                MiningSpeed = 1,
                Description = "A basic pickaxe made from wood.",
                Rarity = ItemRarity.Common,
                Price = 5.0m,
                LevelRequirement = 1,
                AttackValue = 0
            },
            new Pickaxe
            {
                Name = "Stone pickaxe",
                RequiredLevel = 2,
                MiningSpeed = 2,
                Description = "A sturdy pickaxe made from stone.",
                Rarity = ItemRarity.Uncommon,
                Price = 10.0m,
                LevelRequirement = 5,
                AttackValue = 0
            },
            new Pickaxe
            {
                Name = "Diamond pickaxe",
                RequiredLevel = 5,
                MiningSpeed = 3,
                Description = "A high-quality pickaxe made from diamond.",
                Rarity = ItemRarity.Rare,
                Price = 50.0m,
                LevelRequirement = 10,
                AttackValue = 0
            }
            // Add more pickaxe items as needed
        };

        public static async Task SeedData(IServiceProvider serviceProvider)
        {
            var pickaxeService = serviceProvider.GetRequiredService<PickaxeService>();

            
            foreach (var pickaxe in _pickaxes)
            {
                await pickaxeService.InsertAsync(pickaxe);
            }
        }
    }
}
