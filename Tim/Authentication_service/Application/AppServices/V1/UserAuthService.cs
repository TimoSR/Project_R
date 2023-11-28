using _SharedKernel.Patterns.ResultPattern;
using _SharedKernel.Patterns.ResultPattern._Enums;
using Application.AppServices._Interfaces;
using Domain._Shared.Events.UserAuthentication;
using Domain._Shared.Events.UserManagement;
using Domain.UserAuthentication.Entities;
using Domain.UserAuthentication.Repositories;
using Infrastructure.Persistence._Interfaces;
using Infrastructure.Utilities._Interfaces;

namespace Application.AppServices.V1;

public class UserAuthService : IAuthAppServiceV1
{
    private readonly IAuthRepository _authRepository;
    private readonly ITokenHandler _tokenHandler;
    private readonly ILogger<UserAuthService> _logger;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEventHandler _eventHandler;
    private readonly ICacheManager _cacheManager;
    
    private const string TokenCachePrefix = "authToken_";

    public UserAuthService(
        IAuthRepository authRepository,
        ITokenHandler tokenHandler,
        ILogger<UserAuthService> logger,
        IPasswordHasher passwordHasher,
        IEventHandler eventHandler,
        ICacheManager cacheManager
        )
    {
        _authRepository = authRepository;
        _tokenHandler = tokenHandler;
        _eventHandler = eventHandler;
        _cacheManager = cacheManager;
        _logger = logger;
        _passwordHasher = passwordHasher;
    }
    
    private async Task<ServiceResult> LogAndPublishAuthDetailsSetFailure(string email, string reason)
    {
        _logger.LogWarning("Failed to set authentication details for user with Email: {Email}", email);
        await _eventHandler.PublishProtobufEventAsync(new UserAuthDetailsSetFailedEvent { Email = email, Reason = reason });
        return ServiceResult.Failure(reason);
    }
    
    private async Task<ServiceResult> LogAndPublishUserDeletionFailure(string email, string reason)
    {
        _logger.LogWarning("Failed to set authentication details for user with Email: {Email}", email);
        await _eventHandler.PublishProtobufEventAsync(new UserDeletionFailedEvent { Email = email, Reason = reason });
        return ServiceResult.Failure(reason);
    }
    
    public async Task<ServiceResult<(string NewToken, string NewRefreshToken)>?> RefreshTokenAsync(string refreshToken)
    {
        var userId = await _authRepository.ValidateRefreshTokenAsync(refreshToken);

        if (string.IsNullOrEmpty(userId))
        {
            return ServiceResult<(string, string)>.Failure("Invalid or expired refresh token", ServiceErrorType.Unauthorized);
        }

        // Optionally: Revoke the old refresh token
        await _authRepository.UpdateRefreshTokenAsync(userId, null);

        // Generate a new JWT token
        var newToken = _tokenHandler.GenerateJwtToken(userId);

        // Optionally: Generate a new refresh token and store it
        var newRefreshToken = _tokenHandler.GenerateRefreshToken();

        await _authRepository.UpdateRefreshTokenAsync(userId, newRefreshToken);

        return ServiceResult<(string, string)>.Success((newToken, newRefreshToken), "Token refreshed successfully");
    }
    
    public async Task<ServiceResult<(string Token, string RefreshToken)>> LoginAsync(string email, string password)
    {
        var user = await _authRepository.FindByEmailAsync(email);
        if (user == null || !_passwordHasher.VerifyHashedPassword(user.HashedPassword, password))
        {
            return ServiceResult<(string, string)>.Failure("Authentication failed", ServiceErrorType.Unauthorized);
        }

        // Check if tokens are already cached
        var cachedTokens = await _cacheManager.GetValueAsync(TokenCachePrefix + user.Id);
        if (cachedTokens.IsSuccess && cachedTokens.Value != null)
        {
            return ServiceResult<(string, string)>.Success((cachedTokens.Value, null), "Cached login successful");
        }

        // Generate new tokens
        var accessToken = _tokenHandler.GenerateJwtToken(user.Id);
        var refreshToken = _tokenHandler.GenerateRefreshToken();

        // Cache the new tokens
        await _cacheManager.SetValueAsync(TokenCachePrefix + user.Id, accessToken, TimeSpan.FromMinutes(60)); // Set appropriate expiration

        await _authRepository.UpdateRefreshTokenAsync(user.Id, refreshToken);

        return ServiceResult<(string, string)>.Success((accessToken, refreshToken), "Login successful");
    }

    public async Task<ServiceResult> SetUserAuthDetailsAsync(UserRegInitEvent userAuthDetails)
    {
        try
        {
            _logger.LogInformation("Setting authentication details for user with Email: {Email}", userAuthDetails.Email);

            var authUser = new AuthUser
            {
                Email = userAuthDetails.Email,
                HashedPassword = _passwordHasher.HashPassword(userAuthDetails.Password)
            };

            if (!await _authRepository.SetUserAuthDetails(authUser))
            {
                return await LogAndPublishAuthDetailsSetFailure(userAuthDetails.Email, "Failed to set authentication details in the repository.");
            }

            _logger.LogInformation("Authentication details set successfully for user with Email: {Email}", userAuthDetails.Email);
            await _eventHandler.PublishProtobufEventAsync(new UserAuthDetailsSetSuccessEvent { Email = userAuthDetails.Email });

            return ServiceResult.Success("Authentication details set successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while setting authentication details for user with Email: {Email}", userAuthDetails.Email);
            return ServiceResult.Failure("An unexpected error occurred while setting authentication details.", ServiceErrorType.InternalError);
        }
    }
    
    public async Task<ServiceResult> DeleteUserAuthDetailsAsync(UserDeletionInitEvent deletionEvent)
    {
        try
        {
            _logger.LogInformation("Attempting to delete user with Email: {Email}", deletionEvent.Email);
    
            if (!await _authRepository.DeleteUserByEmailAsync(deletionEvent.Email))
            {
                return await LogAndPublishUserDeletionFailure(deletionEvent.Email, "Failed to delete user from the repository.");
            }
    
            _logger.LogInformation("User deleted successfully with Email: {Email}", deletionEvent.Email);
            await _eventHandler.PublishProtobufEventAsync(new UserDeletionSuccessEvent { Email = deletionEvent.Email });
            return ServiceResult.Success("User deleted successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting user with Email: {Email}", deletionEvent.Email);
            return ServiceResult.Failure("An unexpected error occurred while deleting user.", ServiceErrorType.InternalError);
        }
    }

    public async Task<bool> LogoutAsync(string userId)
    {
        _logger.LogInformation("Attempting to logout user with ID: {UserId}", userId);

        // Invalidate the refresh token (optional)
        await _authRepository.UpdateRefreshTokenAsync(userId, null);

        // Remove tokens from cache
        await _cacheManager.RemoveValueAsync(TokenCachePrefix + userId);

        _logger.LogInformation("Successfully logged out user with ID: {UserId}", userId);

        return true;
    }
}
