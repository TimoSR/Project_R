using Application.AppServices._Interfaces;
using Domain.DomainModels;
using Domain.DomainModels.Enums;
using Domain.IRepositories;
using Infrastructure.Utilities._Interfaces;
namespace Application.AppServices;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEmailValidator _emailValidator;
    private readonly IPasswordValidator _passwordValidator;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IEmailValidator emailValidator,
        IPasswordValidator passwordValidator,
        ITokenGenerator tokenGenerator,
        ILogger<AuthService> logger
        )
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _emailValidator = emailValidator;
        _passwordValidator = passwordValidator;
        _tokenGenerator = tokenGenerator;
        _logger = logger;
    }

    public async Task<string> AuthenticateAsync(string email, string password)
    {
        _logger.LogInformation("Attempting to authenticate user with email: {Email}", email);

        if (!_emailValidator.IsValid(email))
        {
            _logger.LogWarning("Invalid email provided: {Email}", email);
            return null;
        }

        var user = await _userRepository.FindByEmailAsync(email);

        if (user == null || !_passwordHasher.VerifyHashedPassword(user, password))
        {
            _logger.LogWarning("Authentication failed for email: {Email}", email);
            return null;
        }

        // Generate JWT token
        var token = _tokenGenerator.GenerateToken(user.Id);

        _logger.LogInformation("Successfully authenticated user with email: {Email}", email);

        return token;
    }

    public async Task<UserRegistrationResult> RegisterAsync(User newUser)
    {
        _logger.LogInformation("Attempting to register new user with email: {Email}", newUser.Email);

        if (!_emailValidator.IsValid(newUser.Email))
        {
            _logger.LogWarning("Invalid email provided for registration: {Email}", newUser.Email);
            return UserRegistrationResult.InvalidEmail;
        }

        var existingUser = await _userRepository.FindByEmailAsync(newUser.Email);
        
        if (existingUser != null)
        {
            _logger.LogWarning("Email already exists: {Email}", newUser.Email);
            return UserRegistrationResult.EmailAlreadyExists;
        }
        
        if (!_passwordValidator.IsValid(newUser.Password))
        {
            _logger.LogWarning("Invalid password provided for email: {Email}", newUser.Email);
            return UserRegistrationResult.InvalidPassword;
        }

        // Hash the password
        newUser.Password = _passwordHasher.HashPassword(newUser);

        // Insert new user
        await _userRepository.CreateUserAsync(newUser);

        _logger.LogInformation("Successfully registered new user with email: {Email}", newUser.Email);

        return UserRegistrationResult.Successful;
    }

    public async Task<bool> LogoutAsync(string userId)
    {
        _logger.LogInformation("Attempting to logout user with ID: {UserId}", userId);

        // Invalidate the refresh token (optional)
        await _userRepository.UpdateRefreshTokenAsync(userId, null, DateTime.UtcNow);

        _logger.LogInformation("Successfully logged out user with ID: {UserId}", userId);

        return true;
    }
    
    public async Task<bool> DeleteUserAsync(string userId)
    {
        _logger.LogInformation("Attempting to delete user with ID: {UserId}", userId);

        // Deleting the user
        await _userRepository.DeleteUserAsync(userId);

        _logger.LogInformation("Successfully deleted user with ID: {UserId}", userId);

        return true;
    }
}
