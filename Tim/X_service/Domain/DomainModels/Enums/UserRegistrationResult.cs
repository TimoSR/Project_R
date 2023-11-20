namespace Domain.DomainModels.Enums;

public enum UserRegistrationResult
{
    Successful,
    EmailAlreadyExists,
    InvalidEmail,
    InvalidPassword
}