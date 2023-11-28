using _SharedKernel.Patterns.ResultPattern;
using _SharedKernel.Patterns.ResultPattern._Enums;
using Application.AppServices._Interfaces;
using Domain.UserAuthentication.Repositories;
using Infrastructure.Utilities._Interfaces;

namespace Application.AppServices.V1;

public class AuthService : IAuthAppServiceV1
{
    private readonly IAuthRepository _userRepository;
    private readonly ITokenHandler _tokenHandler;
    private readonly ILogger<AuthService> _logger;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(
        IAuthRepository userRepository,
        ITokenHandler tokenHandler,
        ILogger<AuthService> logger,
        IPasswordHasher passwordHasher
        )
    {
        _userRepository = userRepository;
        _tokenHandler = tokenHandler;
        _logger = logger;
        _passwordHasher = passwordHasher;
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
        var user = await _userRepository.FindByEmailAsync(email);
        
        if (user == null || !_passwordHasher.VerifyHashedPassword(user.Password, password))
        {
            return ServiceResult<(string, string)>.Failure("Authentication failed", ServiceErrorType.Unauthorized);
        }

        var accessToken = _tokenHandler.GenerateJwtToken(user.Id);
        var refreshToken = _tokenHandler.GenerateRefreshToken();

        await _userRepository.UpdateRefreshTokenAsync(user.Id, refreshToken);

        return ServiceResult<(string, string)>.Success((accessToken, refreshToken), "Login successful");
    }

    public async Task<bool> LogoutAsync(string userId)
    {
        _logger.LogInformation("Attempting to logout user with ID: {UserId}", userId);

        // Invalidate the refresh token (optional)
        await _userRepository.UpdateRefreshTokenAsync(userId, null);

        _logger.LogInformation("Successfully logged out user with ID: {UserId}", userId);

        return true;
    }
}
