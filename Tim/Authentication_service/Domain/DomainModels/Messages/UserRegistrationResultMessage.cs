namespace Domain.DomainModels.Messages;

public class UserRegistrationResultMessage
{
    public readonly string Successful = "User successfully registered";
    public readonly string InvalidEmail = "Invalid email address";
    public readonly string EmailAlreadyExists = "Email already exists";
    public readonly string InvalidPassword = $"Password must have a minimum length of {User.MinPasswordLength} and include at least one uppercase letter, number, and special symbol (e.g., !@#$%^&*).";
}