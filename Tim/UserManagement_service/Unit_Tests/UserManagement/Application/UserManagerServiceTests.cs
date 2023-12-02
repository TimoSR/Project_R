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
using Unit_Tests.UserManagement.Application.TestSetup;

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
    
    [Theory]
    [MemberData(nameof(UserManagerServiceTestCases.RegistrationTestCases), MemberType = typeof(UserManagerServiceTestCases))]
    public async Task RegisterAsync_VariousScenarios_ReturnsExpectedResult(UserRegisterDto userDto, bool expectedResult, int eventPublishTimes)
    {
        // Arrange
        var validationResult = new ValidationResult();
        _mockUserValidationService.Setup(s => s.ValidateNewUser(userDto.Email, userDto.Password))
            .Returns(validationResult);
        _mockUserRepository.Setup(r => r.CreateUserIfNotRegisteredAsync(It.Is<User>(u => u.Email == userDto.Email)))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _userManagerService.RegisterAsync(userDto);

        // Assert
        result.IsSuccess.Should().Be(expectedResult);
        _mockEventHandler.Verify(e => e.PublishProtobufEventAsync(It.IsAny<UserRegInitEvent>()), Times.Exactly(eventPublishTimes));
    }
}