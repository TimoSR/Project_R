using Application.AppServices.V1._Interfaces;
using Application.AppServices.V2._Interfaces;
using Domain.DomainModels;
using Domain.DomainModels.Enums;
using Domain.IRepositories;
using Infrastructure.Utilities._Interfaces;

namespace Application.AppServices.V2;

public class AuthService : IAuthServiceV2
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEmailValidator _emailValidator;
    private readonly IPasswordValidator _passwordValidator;
    private readonly ITokenHandler _tokenHandler;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IEmailValidator emailValidator,
        IPasswordValidator passwordValidator,
        ITokenHandler tokenHandler,
        ILogger<AuthService> logger
        )
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _emailValidator = emailValidator;
        _passwordValidator = passwordValidator;
        _tokenHandler = tokenHandler;
        _logger = logger;
    }
    
    public async Task<(string NewToken, string NewRefreshToken)?> RefreshTokenAsync(string refreshToken)
    {
        try
        {
            var userId = await _userRepository.ValidateRefreshTokenAsync(refreshToken);
        
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning($"Invalid or expired refresh token");
                return null;
            }
        
            // Optionally: Revoke the old refresh token
            await _userRepository.UpdateRefreshTokenAsync(userId, null);

            // Generate a new JWT token
            var newToken = _tokenHandler.GenerateJwtToken(userId);

            // Optionally: Generate a new refresh token and store it
            var newRefreshToken = _tokenHandler.GenerateRefreshToken();
        
            await _userRepository.UpdateRefreshTokenAsync(userId, newRefreshToken);

            return (NewToken: newToken, NewRefreshToken: newRefreshToken);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to refresh token: {ex.Message}");
            return null;
        }
    }


    public async Task<(string Token, string RefreshToken)?> LoginAsync(string email, string password)
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

        // Generate Tokens
        var accessToken = _tokenHandler.GenerateJwtToken(user.Id);
        var refreshToken = _tokenHandler.GenerateRefreshToken();
        
        await _userRepository.UpdateRefreshTokenAsync(user.Id, refreshToken); // Assume 6 hours expiration for refresh tokens

        _logger.LogInformation("Successfully authenticated user with email: {Email}", email);

        return (accessToken, refreshToken);
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
        await _userRepository.UpdateRefreshTokenAsync(userId, null);

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
