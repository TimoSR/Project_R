using _SharedKernel.Patterns.ResultPattern;
using _SharedKernel.Patterns.ResultPattern._Enums;
using Application.AppServices.V1;
using Application.DTO.UserManagement;
using Domain._Shared.Events.Topics.UserManagement;
using Domain.UserManagement.Entities;
using Domain.UserManagement.Repositories;
using Domain.UserManagement.Services._Interfaces;
using FluentAssertions;
using Infrastructure.Persistence._Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace Unit_Tests.UserManagement.Application;

public class UserManagerServiceTests
{
    private readonly Mock<IUserValidationService> _mockUserValidationService;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<ILogger<UserManagerService>> _mockLogger;
    private readonly Mock<IEventHandler> _mockEventHandler;
    private readonly Mock<ICacheManager> _mockCacheManager;
    private readonly UserManagerService _userManagerService;

    public UserManagerServiceTests()
    {
        _mockUserValidationService = new Mock<IUserValidationService>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockLogger = new Mock<ILogger<UserManagerService>>();
        _mockEventHandler = new Mock<IEventHandler>();
        _mockCacheManager = new Mock<ICacheManager>();

        _userManagerService = new UserManagerService(
            _mockUserValidationService.Object,
            _mockUserRepository.Object,
            _mockLogger.Object,
            _mockEventHandler.Object,
            _mockCacheManager.Object
        );
    }

    [Fact]
    public async Task RegisterAsync_SuccessfulRegistration_ReturnsSuccessResult()
    {
        // Arrange
        var userDto = new UserRegisterDto { Email = "test@example.com", UserName = "testUser", Password = "password" };
    
        // Create a successful ValidationResult instance
        var validationResult = new ValidationResult(); // By default, it's successful

        _mockUserValidationService.Setup(s => s.ValidateNewUser(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(validationResult);
        _mockUserRepository.Setup(r => r.CreateUserIfNotRegisteredAsync(It.IsAny<User>()))
            .ReturnsAsync(true);

        // Act
        var result = await _userManagerService.RegisterAsync(userDto);

        // Assert
        Assert.True(result.IsSuccess);
        _mockEventHandler.Verify(e => e.PublishProtobufEventAsync(It.IsAny<UserRegInitEvent>()), Times.Once);
    }
}