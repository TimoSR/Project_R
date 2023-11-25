using _CommonLibrary.Patterns._Enums;
using _CommonLibrary.Patterns.ResultPattern;
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
    private readonly ILogger<AuthService> _logger;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(
        UserValidationService userValidationService,
        IUserRepository userRepository,
        ILogger<AuthService> logger,
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
        var email = userDto.Email;
        var password = userDto.Password;
        
        var validationResult = await _userValidationService.ValidateNewUserAsync(email, password);
        
        if (!validationResult.IsSuccess)
        {
            return ServiceResult.Failure(validationResult.Message, ServiceErrorType.BadRequest);
        }
    
        var newUser = new User
        {
            Email = userDto.Email,
            Password = _passwordHasher.HashPassword(userDto.Password)
        };

        await _userRepository.CreateUserAsync(newUser);

        return ServiceResult.Success(UserRegMsg.Successful);
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