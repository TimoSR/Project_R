using System.Text;
using Newtonsoft.Json;
using x_endpoints.DomainModels._Interfaces;
using x_endpoints.DomainModels.Equipments;
using x_endpoints.DomainModels.Items;
using x_endpoints.DomainServices;
using x_endpoints.Enums;

namespace Headless_client.Application;

enum MenuOption
{
    Login = 1,
    CreateNewUser,
    //CreateNewGame,
    //PlayGame,
    //Exit
}
    
public class Program 
{
    static HttpClient httpClient = new HttpClient();
    static string apiBaseAddress = "https://localhost:7076/api/v1";
    static string accessToken = string.Empty;
    static string refreshToken = string.Empty;

    static async Task Main(string[] args)
    {
        Dictionary<MenuOption, Func<Task>> menuOptions = new Dictionary<MenuOption, Func<Task>>
        {
            { MenuOption.CreateNewUser, CreateNewUserAsync },
            { MenuOption.Login, LoginAsync }
        };

        while (true)
        {
            Console.WriteLine("Welcome to My Game!");

            foreach (MenuOption option in Enum.GetValues(typeof(MenuOption)))
            {
                Console.WriteLine($"{(int)option}. {option}");
            }

            if (int.TryParse(Console.ReadLine(), out int numericOption) && Enum.IsDefined(typeof(MenuOption), numericOption))
            {
                MenuOption selectedOption = (MenuOption)numericOption;
                await menuOptions[selectedOption]();
            }
            else
            {
                Console.WriteLine("Invalid option");
            }
        }
    }

    static async Task CreateNewUserAsync()
    {
        Console.Write("Enter your email: ");
        var email = Console.ReadLine();
        
        Console.Write("Enter your password: ");
        var password = Console.ReadLine();
        
        var userDto = new
        {
            Email = email,
            Password = password
        };

        var content = new StringContent(JsonConvert.SerializeObject(userDto), Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync($"{apiBaseAddress}/auth/register", content);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("User successfully created.");
        }
        else
        {
            Console.WriteLine("Failed to create user.");
        }
    }

    static async Task LoginAsync()
    {
        Console.Write("Username: ");
        var username = Console.ReadLine();

        Console.Write("Password: ");
        var password = Console.ReadLine();

        var tokens = await AuthenticateAsync(username, password);
        
        if (tokens != null)
        {
            accessToken = tokens.AccessToken;
            refreshToken = tokens.RefreshToken;
            httpClient.DefaultRequestHeaders.Authorization = new ("Bearer", accessToken);
            Console.WriteLine("Successfully logged in!");
            Console.WriteLine(accessToken);
        }
        else
        {
            Console.WriteLine("Invalid username or password.");
        }
    }

    static async Task<LoginResponseDto> AuthenticateAsync(string username, string password)
    {
        var loginRequestDto = new
        {
            Email = username,
            Password = password
        };

        var content = new StringContent(JsonConvert.SerializeObject(loginRequestDto), Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync($"{apiBaseAddress}/auth/login", content);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<LoginResponseDto>(responseContent);
        }

        return null;
    }

    static async Task<bool> RefreshTokenAsync()
    {
        var refreshRequestDto = new
        {
            RefreshToken = refreshToken
        };

        var content = new StringContent(JsonConvert.SerializeObject(refreshRequestDto), Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync($"{apiBaseAddress}/auth/refresh-token", content);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var refreshedTokens = JsonConvert.DeserializeObject<LoginResponseDto>(responseContent);
            accessToken = refreshedTokens.AccessToken;
            refreshToken = refreshedTokens.RefreshToken;
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            return true;
        }

        return false;
    }

    static void CreateChest()
    {

        var chests = new List<Armor>
        {
            new Armor
            {
                Name = "Iron Chestplate",
                LevelRequirement = 10,
                ArmorValue = 20,
                Slot = EquipmentSlot.Chest,
                Rarity = ItemRarity.Common // Assign the rarity using the enum
            },
            new Armor
            {
                Name = "Steel Breastplate",
                LevelRequirement = 15,
                ArmorValue = 25,
                Slot = EquipmentSlot.Chest,
                Rarity = ItemRarity.Uncommon // Assign the rarity using the enum
            },
            new Armor
            {
                Name = "Enchanted Chestguard",
                LevelRequirement = 20,
                ArmorValue = 30,
                Slot = EquipmentSlot.Chest,
                Rarity = ItemRarity.Rare // Assign the rarity using the enum
            },
            // Add more chest armor items as needed
        };
    }

    static void CreatePickaxes()
    {
     List<Pickaxe> _pickaxes = new List<Pickaxe>
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
     foreach (Pickaxe pickaxe in _pickaxes)
     {
         Console.WriteLine(pickaxe.ToString());
     }

    }
    static void CreateNewBars()
    {
        
        var bars = new List<Bar>
        {
            new Bar
            {
                Name = "Golden Metal Bar",
                Description = "A bar made of pure gold",
                Price = 1500,
                Capacity = 100,
                Rating = 4.5,
                Rarity = ItemRarity.Epic // Assign the rarity using the enum
            },
            new Bar
            {
                Name = "Ironworks Bar",
                Description = "A sturdy bar made from iron",
                Price = 300,
                Capacity = 50,
                Rating = 4.0,
                Rarity = ItemRarity.Common // Assign the rarity using the enum
            },
            new Bar
            {
                Name = "Diamond-Encrusted Bar",
                Description = "A lavish bar with diamond accents",
                Price = 2500,
                Capacity = 75,
                Rating = 4.8,
                Rarity = ItemRarity.Legendary // Assign the rarity using the enum
            },
            // Add more bar items as needed
        };

        foreach (Bar bar in bars)
        {
            Console.WriteLine(bar.ToString());
        }
    }
    
    static void CreateNewGame()
    {
        RefreshTokenAsync().Wait();
        Console.WriteLine("Creating a new game...");
        // Placeholder for your game creation logic
    }

    static void PlayGame()
    {
        RefreshTokenAsync().Wait();
        Console.WriteLine("Playing the game...");
        // Placeholder for your game logic
    }
    
    // Create Item class using IItems Interface 
    
    public class ExampleItem : IItems
    {
        public string Id { get; set; }
        public ItemRarity Rarity { get; set; }
        // In ItemRarity there is the following options 
        //Common,
        //Uncommon,
       // Rare,
        //Epic,
        //Legendary
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        // fill with your needs 
    }
    
    // Creating Equipment using IItems and IEquipment 
    public class ArmorType : IItems, IEquipment 
    {
        public string Id { get; set; }
        public ItemRarity Rarity { get; set; }
        // In ItemRarity there is the following options 
        //Common,
        //Uncommon,
        // Rare,
        //Epic,
        //Legendary
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int LevelRequirement { get; set; }
        public EquipmentSlot Slot { get; }
        // in EquipmentSlot  there is the following options 
        //Head,
        //Chest,
       // Legs,
        //Weapon,
    }
    public class LoginResponseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
    
}
