using _CommonLibrary.Patterns.ResultPattern;
using _CommonLibrary.Patterns.ResultPattern._Enums;
using Application.AppServices._Interfaces;
using Application.DTO.Auth;
using Application.DTO.UserManagement;
using Domain.User.Entities;
using Domain.User.Messages;
using Domain.User.Repositories;
using Domain.User.Services;
using Infrastructure.Utilities._Interfaces;

namespace Application.AppServices.V1;

public class UserService : IUserService
{
    private readonly UserValidationService _userValidationService;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(
        UserValidationService userValidationService,
        IUserRepository userRepository,
        ILogger<UserService> logger,
        IPasswordHasher passwordHasher
    )
    {
        _userValidationService = userValidationService;
        _userRepository = userRepository;
        _logger = logger;
        _passwordHasher = passwordHasher;
    }
    
    public async Task<ServiceResult> RegisterAsync(UserRegisterDto userDto)
    {
        _logger.LogInformation("Starting user registration process for Email: {Email}", userDto.Email);
        
        try
        {
            //
            var validationResult = _userValidationService.ValidateNewUser(userDto.Email, userDto.Password);
            if (!validationResult.IsSuccess)
            {
                _logger.LogWarning("User registration validation failed for Email: {Email} - Reasons: {Reasons}", userDto.Email, string.Join(", ", validationResult.Messages));
                return ServiceResult.Failure(validationResult.Messages, ServiceErrorType.BadRequest);
            }

            var newUser = new User
            {
                Email = userDto.Email,
                Password = _passwordHasher.HashPassword(userDto.Password)
            };
            
            //
            bool registrationSuccess = await _userRepository.CreateUserIfNotRegisteredAsync(newUser);
            if (!registrationSuccess)
            {
                _logger.LogWarning("User registration failed for Email: {Email} - Email already exists", userDto.Email);
                return ServiceResult.Failure(UserRegMsg.EmailAlreadyExists, ServiceErrorType.BadRequest);
            }
            //

            _logger.LogInformation("User registration completed successfully for Email: {Email}", newUser.Email);
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
            Id = user.Id,
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
}