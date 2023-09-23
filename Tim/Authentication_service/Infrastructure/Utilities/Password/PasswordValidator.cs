using System.Text.RegularExpressions;

namespace Infrastructure.Utilities.Password;

public class PasswordValidator
{
    // Minimum length required for the password
    private const int MinimumLength = 6;

    // Regular expressions for various types of characters
    private static readonly Regex UpperCase = new Regex("[A-Z]");
    private static readonly Regex LowerCase = new Regex("[a-z]");
    private static readonly Regex Digits = new Regex("[0-9]");
    private static readonly Regex SpecialChars = new Regex("[!@#$%^&*(),.?\":{}|<>]");

    public static bool IsValid(string password)
    {
        // Check minimum length
        if (string.IsNullOrEmpty(password) || password.Length < MinimumLength)
        {
            return false;
        }

        // Check for upper-case letters
        if (!UpperCase.IsMatch(password))
        {
            return false;
        }

        // Check for lower-case letters
        if (!LowerCase.IsMatch(password))
        {
            return false;
        }

        // Check for digits
        if (!Digits.IsMatch(password))
        {
            return false;
        }

        // Check for special characters
        if (!SpecialChars.IsMatch(password))
        {
            return false;
        }

        return true;
    }
}