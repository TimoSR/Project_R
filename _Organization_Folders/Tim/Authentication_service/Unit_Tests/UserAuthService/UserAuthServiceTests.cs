using _SharedKernel.Patterns.ResultPattern;
using _SharedKernel.Patterns.ResultPattern._Enums;
using Domain._Shared.Events.Subscribed.UserManagement;
using Domain._Shared.Events.Topics.UserAuthentication;
using Domain.UserAuthentication.Entities;
using Domain.UserAuthentication.Repositories;
using FluentAssertions;
using Infrastructure.Persistence._Interfaces;
using Infrastructure.Utilities._Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Unit_Tests.UserAuthService.TestSetup;

namespace Unit_Tests.UserAuthService;

public class UserAuthServiceTests
{
        private readonly Application.AppServices.V1.UserAuthService _userAuthService;
        private readonly Mock<IAuthRepository> _authRepositoryMock;
        private readonly Mock<ITokenHandler> _tokenHandlerMock;
        private readonly Mock<ILogger<Application.AppServices.V1.UserAuthService>> _loggerMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<IEventHandler> _eventHandlerMock;
        private readonly Mock<ICacheManager> _cacheManagerMock;

        public UserAuthServiceTests()
        {
            _authRepositoryMock = new Mock<IAuthRepository>();
            _tokenHandlerMock = new Mock<ITokenHandler>();
            _loggerMock = new Mock<ILogger<Application.AppServices.V1.UserAuthService>>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _eventHandlerMock = new Mock<IEventHandler>();
            _cacheManagerMock = new Mock<ICacheManager>();

            _userAuthService = new Application.AppServices.V1.UserAuthService(
                _authRepositoryMock.Object,
                _tokenHandlerMock.Object,
                _loggerMock.Object,
                _passwordHasherMock.Object,
                _eventHandlerMock.Object,
                _cacheManagerMock.Object);
            
            _tokenHandlerMock.Setup(x => x.GenerateJwtToken(It.IsAny<string>())).Returns("expectedToken");
            _tokenHandlerMock.Setup(x => x.GenerateRefreshToken()).Returns("expectedRefreshToken");

            // Mock ILogger
            _loggerMock.Setup(log => log.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>())
            ).Callback<LogLevel, EventId, object, Exception, Delegate>((logLevel, eventId, state, exception, func) =>
            {
                // Optional logic for specific log messages
            });
        }

        [Theory]
        [MemberData(nameof(UserAuthServiceTestCases.ValidRefreshTokenTestCases), MemberType = typeof(UserAuthServiceTestCases))]
        public async Task RefreshTokenAsync_WithValidToken_ReturnsNewToken(UserAuthServiceTestCases.RefreshTokenTestCase testCase)
        {
            // Arrange
            _authRepositoryMock.Setup(x => x.ValidateRefreshTokenAsync(testCase.RefreshToken)).ReturnsAsync(testCase.UserId);
            _tokenHandlerMock.Setup(x => x.GenerateJwtToken(testCase.UserId)).Returns(testCase.NewToken);
            _tokenHandlerMock.Setup(x => x.GenerateRefreshToken()).Returns(testCase.NewRefreshToken);

            // Act
            var result = await _userAuthService.RefreshTokenAsync(testCase.RefreshToken);

            // Assert
            result.Data.Item1.Should().Be(testCase.NewToken);
            result.Data.Item2.Should().Be(testCase.NewRefreshToken);
            result.IsSuccess.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(UserAuthServiceTestCases.InvalidRefreshTokenTestCases), MemberType = typeof(UserAuthServiceTestCases))]
        public async Task RefreshTokenAsync_WithInvalidToken_ReturnsFailure(UserAuthServiceTestCases.RefreshTokenTestCase testCase)
        {
            // Arrange
            _authRepositoryMock.Setup(x => x.ValidateRefreshTokenAsync(testCase.RefreshToken)).ReturnsAsync(testCase.UserId);

            // Act
            var result = await _userAuthService.RefreshTokenAsync(testCase.RefreshToken);

            // Assert
            result.Should().NotBeNull();
            result?.IsSuccess.Should().BeFalse();
            result?.ErrorType.Should().Be(ServiceErrorType.Unauthorized);
        }
        
        [Theory]
        [MemberData(nameof(UserAuthServiceTestCases.LoginTestCases), MemberType = typeof(UserAuthServiceTestCases))]
        public async Task LoginAsync_ReturnsExpectedResult(UserAuthServiceTestCases.LoginTestCase testCase)
        {
            _authRepositoryMock.Setup(x => x.FindByEmailAsync(It.Is<string>(s => string.IsNullOrEmpty(s))))
                .ReturnsAsync((AuthUser)null); // Handles null or empty emails.

            _authRepositoryMock.Setup(x => x.FindByEmailAsync(It.Is<string>(s => s == "nonexistent@example.com")))
                .ReturnsAsync((AuthUser)null); // Handles specific case where email is known to be nonexistent.

            _passwordHasherMock.Setup(x => x.VerifyHashedPassword(It.IsAny<string>(), It.Is<string>(s => string.IsNullOrEmpty(s))))
                .Returns(false); // Handles null or empty passwords.

            _passwordHasherMock.Setup(x => x.VerifyHashedPassword(It.IsAny<string>(), It.Is<string>(s => s == "InvalidPassword")))
                .Returns(false); // Handles specific case where password is known to be invalid.

            // Setup for valid case
            _authRepositoryMock.Setup(x => x.FindByEmailAsync(It.Is<string>(s => s == "validuser@example.com")))
                .ReturnsAsync(new AuthUser { Email = "validuser@example.com", HashedPassword = _passwordHasherMock.Object.HashPassword("ValidPassword") });

            _passwordHasherMock.Setup(x => x.VerifyHashedPassword(It.IsAny<string>(), It.Is<string>(s => s == "ValidPassword")))
                .Returns(true); // Assumes the password "ValidPassword" is correct for a valid user.

            // Act
            var result = await _userAuthService.LoginAsync(testCase.Email, testCase.Password);

            // Assert
            result.Should().BeEquivalentTo(testCase.ExpectedResult, options => options.ComparingByMembers<ServiceResult<(string, string)>>());
        }
        
        [Theory]
        [MemberData(nameof(UserAuthServiceTestCases.SetUserAuthDetailsTestCases), MemberType = typeof(UserAuthServiceTestCases))]
        public async Task SetUserAuthDetailsAsync_ReturnsExpectedResult(UserAuthServiceTestCases.SetUserAuthDetailsTestCase testCase)
        {
            // Arrange
            _authRepositoryMock.Setup(x => x.SetUserAuthDetails(It.IsAny<AuthUser>())).ReturnsAsync(testCase.ExpectedSuccess);

            // Act
            var result = await _userAuthService.SetUserAuthDetailsAsync(testCase.UserAuthDetails);

            // Assert
            if (testCase.ExpectedSuccess)
            {
                result.IsSuccess.Should().BeTrue();
                _eventHandlerMock.Verify(x => x.PublishProtobufEventAsync(It.IsAny<UserAuthDetailsSetSuccessEvent>()), Times.Once);
            }
            else
            {
                result.IsSuccess.Should().BeFalse();
                _eventHandlerMock.Verify(x => x.PublishProtobufEventAsync(It.IsAny<UserAuthDetailsSetFailedEvent>()), Times.Once);
            }
        }

        [Fact]
        public async Task SetUserAuthDetailsAsync_WhenExceptionOccurs_ReturnsFailure()
        {
            // Arrange
            var userAuthDetails = new UserRegInitEvent { Email = "test@example.com", Password = "password" };
            _authRepositoryMock.Setup(x => x.SetUserAuthDetails(It.IsAny<AuthUser>())).ThrowsAsync(new Exception());

            // Act
            var result = await _userAuthService.SetUserAuthDetailsAsync(userAuthDetails);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorType.Should().Be(ServiceErrorType.InternalError);
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error, 
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(), 
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()), 
                Times.Once);
        }
        
        [Theory]
        [MemberData(nameof(UserAuthServiceTestCases.DeleteUserAuthDetailsTestCases), MemberType = typeof(UserAuthServiceTestCases))]
        public async Task DeleteUserAuthDetailsAsync_ReturnsExpectedResult(UserAuthServiceTestCases.DeleteUserAuthDetailsTestCase testCase)
        {
            // Arrange
            _authRepositoryMock.Setup(x => x.DeleteUserByEmailAsync(testCase.DeletionEvent.Email)).ReturnsAsync(testCase.ExpectedSuccess);

            // Act
            var result = await _userAuthService.DeleteUserAuthDetailsAsync(testCase.DeletionEvent);

            // Assert
            if (testCase.ExpectedSuccess)
            {
                result.IsSuccess.Should().BeTrue();
                _eventHandlerMock.Verify(x => x.PublishProtobufEventAsync(It.IsAny<UserDeletionSuccessEvent>()), Times.Once);
            }
            else
            {
                result.IsSuccess.Should().BeFalse();
                _eventHandlerMock.Verify(x => x.PublishProtobufEventAsync(It.IsAny<UserDeletionFailedEvent>()), Times.Once);
            }
        }

        // Test for handling exceptions in DeleteUserAuthDetailsAsync
        [Fact]
        public async Task DeleteUserAuthDetailsAsync_WhenExceptionOccurs_ReturnsFailure()
        {
            // Arrange
            var deletionEvent = new UserDeletionInitEvent { Email = "test@example.com" };
            _authRepositoryMock.Setup(x => x.DeleteUserByEmailAsync(deletionEvent.Email)).ThrowsAsync(new Exception());

            // Act
            var result = await _userAuthService.DeleteUserAuthDetailsAsync(deletionEvent);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorType.Should().Be(ServiceErrorType.InternalError);
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error, 
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(deletionEvent.Email)), // Check if the logged message contains the 'deletionEvent.Email'
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()), 
                Times.Once); // Verifies that this log was made once
        }
        
        [Fact]
        public async Task LogoutAsync_SuccessfullyLogsOutUser()
        {
            // Arrange
            const string userId = "testUserId";

            // Act
            var result = await _userAuthService.LogoutAsync(userId);

            // Assert
            result.Should().BeTrue();
            _authRepositoryMock.Verify(x => x.UpdateRefreshTokenAsync(userId, null), Times.Once);
            _cacheManagerMock.Verify(x => x.RemoveValueAsync($"authToken_{userId}"), Times.Once);
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information, 
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(userId)), // This checks if the logged message contains the userId
                    null, // Exception parameter is null for information logs
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()), 
                Times.Exactly(2)); // Verifies that this log was made exactly twice

        }
}