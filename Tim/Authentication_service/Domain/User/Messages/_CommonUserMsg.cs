namespace Domain.User.Messages;

public static class _CommonUserMsg
{
    public const string InvalidEmail = "Invalid email address";
    public static readonly string InvalidPassword = $"Password must have a minimum length of {Entities.User.MinPasswordLength} and include at least one uppercase letter, number, and special symbol (e.g., !@#$%^&*).";
}