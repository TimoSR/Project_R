using _SharedKernel.Patterns.ResultPattern;
using _SharedKernel.Patterns.ResultPattern._Enums;
using Application.AppServices._Interfaces;
using Application.DTO.UserManagement;
using Domain._Shared.Events.UserAuthentication;
using Domain._Shared.Events.UserManagement;
using Domain.UserManagement.Entities;
using Domain.UserManagement.Enums;
using Domain.UserManagement.Messages;
using Domain.UserManagement.Repositories;
using Domain.UserManagement.Services;
using Infrastructure.Persistence._Interfaces;
using Infrastructure.Utilities._Interfaces;

namespace Application.AppServices.V1;

public class UserManagerService : IUserService
{
    private readonly UserValidationService _userValidationService;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserManagerService> _logger;
    private readonly IEventHandler _eventHandler;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ICacheManager _cacheManager;

    public UserManagerService(
        UserValidationService userValidationService,
        IUserRepository userRepository,
        ILogger<UserManagerService> logger,
        IEventHandler eventHandler,
        ICacheManager cacheManager,
        IPasswordHasher passwordHasher
    )
    {
        _userValidationService = userValidationService;
        _userRepository = userRepository;
        _logger = logger;
        _eventHandler = eventHandler;
        _cacheManager = cacheManager;
        _passwordHasher = passwordHasher;
    }
    
    public async Task<ServiceResult> RegisterAsync(UserRegisterDto userDto)
    {
        _logger.LogInformation("Starting user registration process for Email: {Email}", userDto.Email);
        
        try
        {
            var validationResult = _userValidationService.ValidateNewUser(userDto.Email, userDto.Password);
            if (!validationResult.IsSuccess)
            {
                _logger.LogWarning("User registration validation failed for Email: {Email} - Reasons: {Reasons}", userDto.Email, string.Join(", ", validationResult.Messages));
                return ServiceResult.Failure(validationResult.Messages, ServiceErrorType.BadRequest);
            }

            var newUser = new User
            {
                Email = userDto.Email,
                UserName = userDto.UserName
            };
            
            bool registrationSuccess = await _userRepository.CreateUserIfNotRegisteredAsync(newUser);
            if (!registrationSuccess)
            {
                _logger.LogWarning("User registration failed for Email: {Email} - Email already exists", userDto.Email);
                return ServiceResult.Failure(UserRegMsg.EmailAlreadyExists, ServiceErrorType.BadRequest);
            }

            _logger.LogInformation("User registration completed successfully for Email: {Email}", newUser.Email);

            var userRegInit = new UserRegInitEvent()
            {
                Email = userDto.Email,
                Password = _passwordHasher.HashPassword(userDto.Password)
            };

            await _eventHandler.PublishProtobufEventAsync(userRegInit);
            return ServiceResult.Success("User successfully registered.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred during the user registration process for Email: {Email}", userDto.Email);
            return ServiceResult.Failure( "An unexpected error occurred.");
        }
    }

    public async Task<ServiceResult<UserDto>> GetUserByIdAsync(string userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
        {
            _logger.LogWarning("User with ID: {UserId} not found.", userId);
            return ServiceResult<UserDto>.Failure("User not found.", ServiceErrorType.NotFound);
        }

        var userDto = new UserDto()
        {
            Email = user.Email
        };
        
        return ServiceResult<UserDto>.Success(userDto);
    }
    
    public async Task<bool> DeleteUserAsync(string userId)
    {
        _logger.LogInformation("Attempting to delete user with ID: {UserId}", userId);

        // Deleting the user
        await _userRepository.DeleteUserAsync(userId);

        _logger.LogInformation("Successfully deleted user with ID: {UserId}", userId);

        return true;
    }
    
    public async Task<ServiceResult> UpdateUserStatusByEmailAsync(UserAuthDetailsSetSuccessEvent @event, UserStatus status)
    {
        _logger.LogInformation("Attempting to update status of user with email: {Email}", @event.Email);

        try
        {
            bool updateSuccess = await _userRepository.UpdateUserStatusByEmailAsync(@event.Email, status);

            if (!updateSuccess)
            {
                _logger.LogWarning("Update of user status failed for email: {Email}", @event.Email);
                return ServiceResult.Failure("User not found or update failed.", ServiceErrorType.NotFound);
            }

            _logger.LogInformation("Status of user with email: {Email} updated successfully to {Status}", @event.Email, status);
            return ServiceResult.Success("User status updated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during the update process for user status with email: {Email}", @event.Email);
            return ServiceResult.Failure("An unexpected error occurred during update.");
        }
    }
    
    // In UserManagerService class
    public async Task<ServiceResult> RollBackUserAsync(UserAuthDetailsSetFailedEvent @event)
    {
        _logger.LogInformation("Attempting to rollback user with email: {Email}", @event.Email);

        try
        {
            bool rollbackSuccess = await _userRepository.RollbackUserByEmailAsync(@event.Email);

            if (!rollbackSuccess)
            {
                _logger.LogWarning("Rollback failed for user with email: {Email}", @event.Email);
                return ServiceResult.Failure("Rollback failed.");
            }

            _logger.LogInformation("Rollback was successful for user with email: {Email}", @event.Email);
            return ServiceResult.Success("Rollback successful.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during the rollback process for user with email: {Email}", @event.Email);
            return ServiceResult.Failure("An unexpected error occurred during rollback.");
        }
    }
}