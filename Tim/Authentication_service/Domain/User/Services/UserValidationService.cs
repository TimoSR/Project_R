using _CommonLibrary.Patterns.RegistrationHooks.Services._Interfaces;
using _CommonLibrary.Patterns.ResultPattern;
using Domain.User.Messages;
using Domain.User.Repositories;
using Domain.User.Validators.Email;
using Domain.User.Validators.Password;

namespace Domain.User.Services;

public class UserValidationService : IDomainService
{
    private readonly EmailValidator _emailValidator;
    private readonly PasswordValidator _passwordValidator;
    private readonly IUserRepository _userRepository;

    public UserValidationService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _emailValidator = new EmailValidator();
        _passwordValidator = new PasswordValidator();
    }

    public ValidationResult ValidateNewUser(string email, string password)
    {
        var validationResult = new ValidationResult();

        if (!_emailValidator.IsValid(email))
        {
            validationResult.AddError(_CommonUserMsg.InvalidEmail);
        }

        if (!_passwordValidator.IsValid(password))
        {
            validationResult.AddError(_CommonUserMsg.InvalidPassword);
        }

        // If there are no errors, validationResult.IsSuccess will be true.
        // If there are errors, validationResult.IsSuccess will be false and Messages will contain the errors.
        return validationResult;
    }
}