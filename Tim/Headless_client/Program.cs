using System;
using System.Collections.Generic;

namespace CustomLoginExample
{
    class Program
    {
        static Dictionary<string, string> users = new Dictionary<string, string>();

        static void Main(string[] args)
        {
            // Populate with some test data
            users.Add("john", "password123");
            users.Add("jane", "supersecret");

            while (true)
            {
                Console.WriteLine("Welcome to My Game!");
                Console.WriteLine("1. Create New Game");
                Console.WriteLine("2. Play Game");
                Console.WriteLine("3. Login");
                Console.WriteLine("4. Exit");

                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        CreateNewGame();
                        break;

                    case "2":
                        PlayGame();
                        break;

                    case "3":
                        Console.Write("Username: ");
                        var username = Console.ReadLine();

                        Console.Write("Password: ");
                        var password = Console.ReadLine();

                        if (ValidateUser(username, password))
                        {
                            Console.WriteLine("Successfully logged in!");
                            // Proceed to the game or whatever functionality
                        }
                        else
                        {
                            Console.WriteLine("Invalid username or password.");
                        }
                        break;

                    case "4":
                        Console.WriteLine("Exiting game. Goodbye!");
                        return; // Exit the loop and end the program

                    default:
                        Console.WriteLine("Invalid option");
                        break;
                }
            }
        }

        static bool ValidateUser(string username, string password)
        {
            // In a real-world application, you'd hash the password and compare it to a stored hash.
            // This is a simplified example.
            if (users.TryGetValue(username, out var storedPassword))
            {
                return storedPassword == password;
            }

            return false;
        }

        static void CreateNewGame()
        {
            Console.WriteLine("Creating a new game...");
            // Placeholder for your game creation logic
        }

        static void PlayGame()
        {
            Console.WriteLine("Playing the game...");
            // Placeholder for your game logic
        }
    }
}
