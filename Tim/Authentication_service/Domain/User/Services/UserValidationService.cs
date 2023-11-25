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

    public async Task<ValidationResult> ValidateNewUserAsync(string email, string password)
    {
        if (!_emailValidator.IsValid(email))
        {
            return ValidationResult.Failure(_CommonUserMsg.InvalidEmail);
        }

        var existingUser = await _userRepository.FindByEmailAsync(email);
        
        if (existingUser != null)
        {
            return ValidationResult.Failure(UserRegMsg.EmailAlreadyExists);
        }

        if (!_passwordValidator.IsValid(password))
        {
            return ValidationResult.Failure(_CommonUserMsg.InvalidPassword);
        }

        return ValidationResult.Success();
    }
    
    

}