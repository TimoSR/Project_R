using System.Text.RegularExpressions;

namespace Domain.ValueObject;

public class EmailAddress
{
    public string Value { get; private set; }

    private static readonly HashSet<string> PopularEmailRegexes = new HashSet<string>
    {
        @"gmail\.com$",  // Matches any string that ends with "gmail.com"
        @"yahoo\.com$",  // Matches any string that ends with "yahoo.com"
        @"hotmail\.com$",  // Matches any string that ends with "hotmail.com"
        @"outlook\.com$",
        @"msn\.com$",
        @"live\.com$",
        @"aol\.com$",
        @"protonmail\.com$",
        @"zoho\.com$",
        @"icloud\.com$",
        @"mail\.ru$",
        @"yandex\.ru$",
        @"inbox\.com$",
        @"gmx\.com$",
        @"fastmail\.com$",
        @"hushmail\.com$",
        @"lycos\.com$",
        @"rediffmail\.com$",
        @"sina\.com$",
        @"qq\.com$"
    };


    public EmailAddress(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
        {
            throw new Exception("Invalid email address");
        }

        Value = email;
    }
    
    private static bool IsValidEmail(string email)
    {
        // Split email into username and domain parts
        var parts = email.Split('@');
        
        if (parts.Length != 2)
        {
            return false;
        }

        // Validate username part
        var usernameRegex = @"^[a-zA-Z0-9._-]+$";
        
        if (!Regex.IsMatch(parts[0], usernameRegex))
        {
            return false;
        }

        // Validate domain part
        if (!PopularEmailRegexes.Any(regex => Regex.IsMatch(parts[1], regex, RegexOptions.IgnoreCase)))
        {
            return false;
        }

        return true;
    }

}