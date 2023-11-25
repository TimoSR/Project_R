using _CommonLibrary.Patterns.ResultPattern;
using _CommonLibrary.Patterns.ResultPattern._Enums;
using Application.AppServices._Interfaces;
using Application.DTO.Auth;
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

        var validationResult = _userValidationService.ValidateNewUserAsync(userDto.Email, userDto.Password);
        if (!validationResult.IsSuccess)
        {
            _logger.LogWarning("User registration validation failed for Email: {Email} - Reason: {Reason}", userDto.Email, validationResult.Message);
            return ServiceResult.Failure(validationResult.Message, ServiceErrorType.BadRequest);
        }

        var newUser = new User
        {
            Email = userDto.Email,
            Password = _passwordHasher.HashPassword(userDto.Password)
        };

        bool registrationSuccess = await _userRepository.RegisterUserIfNotRegisteredAsync(newUser);
        if (!registrationSuccess)
        {
            _logger.LogWarning("User registration failed for Email: {Email} - Email already exists", userDto.Email);
            return ServiceResult.Failure(UserRegMsg.EmailAlreadyExists, ServiceErrorType.BadRequest);
        }

        _logger.LogInformation("User registration completed successfully for Email: {Email}", newUser.Email);
        return ServiceResult.Success("User successfully registered.");  
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