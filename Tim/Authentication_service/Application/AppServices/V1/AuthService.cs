using _CommonLibrary.Patterns.ResultPattern;
using Domain.User.Entities;
using Domain.User.Messages;
using Domain.User.Repositories;
using Domain.User.Services;
using Infrastructure.Utilities._Interfaces;

namespace Application.AppServices.V1;

public class AuthService : IAuthServiceV1
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
    
    public async Task<ServiceResult<(string NewToken, string NewRefreshToken)>?> RefreshTokenAsync(string refreshToken)
    {
        var userId = await _userRepository.ValidateRefreshTokenAsync(refreshToken);

        if (string.IsNullOrEmpty(userId))
        {
            return ServiceResult<(string, string)>.Failure("Invalid or expired refresh token", ServiceErrorType.Unauthorized);
        }

        // Optionally: Revoke the old refresh token
        await _userRepository.UpdateRefreshTokenAsync(userId, null);

        // Generate a new JWT token
        var newToken = _tokenHandler.GenerateJwtToken(userId);

        // Optionally: Generate a new refresh token and store it
        var newRefreshToken = _tokenHandler.GenerateRefreshToken();

        await _userRepository.UpdateRefreshTokenAsync(userId, newRefreshToken);

        return ServiceResult<(string, string)>.Success((newToken, newRefreshToken), "Token refreshed successfully");
    }
    
    public async Task<ServiceResult<(string Token, string RefreshToken)>> LoginAsync(string email, string password)
    {
        if (!_emailValidator.IsValid(email))
        {
            return ServiceResult<(string, string)>.Failure(_CommonUserMsg.InvalidEmail, ServiceErrorType.BadRequest);
        }

        var user = await _userRepository.FindByEmailAsync(email);
        
        if (user == null || !_passwordHasher.VerifyHashedPassword(user, password))
        {
            return ServiceResult<(string, string)>.Failure("Authentication failed", ServiceErrorType.Unauthorized);
        }

        var accessToken = _tokenHandler.GenerateJwtToken(user.Id);
        var refreshToken = _tokenHandler.GenerateRefreshToken();

        await _userRepository.UpdateRefreshTokenAsync(user.Id, refreshToken);

        return ServiceResult<(string, string)>.Success((accessToken, refreshToken), "Login successful");
    }

    public async Task<ServiceResult> RegisterAsync(User newUser)
    {   
        if (!_emailValidator.IsValid(newUser.Email))
        {
            return ServiceResult.Failure(_CommonUserMsg.InvalidEmail, ServiceErrorType.BadRequest);
        }

        var existingUser = await _userRepository.FindByEmailAsync(newUser.Email);
        
        if (existingUser != null)
        {
            return ServiceResult.Failure(UserRegMsg.EmailAlreadyExists, ServiceErrorType.BadRequest);
        }

        if (!_passwordValidator.IsValid(newUser.Password))
        {
            return ServiceResult.Failure(_CommonUserMsg.InvalidPassword, ServiceErrorType.BadRequest);
        }

        // Hash the password
        newUser.Password = _passwordHasher.HashPassword(newUser);

        // Insert new user
        await _userRepository.CreateUserAsync(newUser);

        return ServiceResult.Success(UserRegMsg.Successful);
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
