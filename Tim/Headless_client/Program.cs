using System.Text;
using Newtonsoft.Json;

namespace Headless_client
{
    
    enum MenuOption
    {
        Login = 1,
        CreateNewUser,
        CreateNewGame,
        PlayGame,
        Exit
    }
    
    class Program
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
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
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

        public class LoginResponseDto
        {
            public string AccessToken { get; set; }
            public string RefreshToken { get; set; }
        }
    }
}
