using Application.AppServices.V1;
using Domain.DomainModels;
using Domain.DomainModels.Enums;
using Domain.User.Services._Interfaces;
using Domain.UserManagement.Entities;
using Domain.UserManagement.Repositories;
using FluentAssertions;
using Infrastructure.Utilities._Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Unit_Tests.Authentication.V1.AuthServiceSetup;

namespace Unit_Tests.Authentication.V1
{
    public class AuthServiceTests
    {
        private readonly UserAuthService _userAuthService;
        private readonly AuthServiceTestSetup _testSetup;
        
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<IEmailValidator> _emailValidatorMock;
        private readonly Mock<IPasswordValidator> _passwordValidatorMock;
        private readonly Mock<ITokenHandler> _tokenGeneratorMock;
        private readonly Mock<ILogger<UserAuthService>> _loggerMock;
        
        public AuthServiceTests()
        {
            _testSetup = new AuthServiceTestSetup();
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _emailValidatorMock = new Mock<IEmailValidator>();
            _passwordValidatorMock = new Mock<IPasswordValidator>();
            _tokenGeneratorMock = new Mock<ITokenHandler>();
            _loggerMock = new Mock<ILogger<UserAuthService>>();

            _userAuthService = new UserAuthService(
                _userRepositoryMock.Object, 
                _passwordHasherMock.Object, 
                _emailValidatorMock.Object, 
                _passwordValidatorMock.Object, 
                _tokenGeneratorMock.Object, 
                _loggerMock.Object);

            _testSetup.SetupDefaultMocks(_emailValidatorMock, _passwordValidatorMock, _passwordHasherMock, _tokenGeneratorMock, _userRepositoryMock);
        }
        
        [Theory]
        [MemberData(nameof(AuthServiceTestCases.ValidCredentialsTestCases), MemberType = typeof(AuthServiceTestCases))]
        public async Task LoginAsync_ReturnsToken_WhenValidCredentials(AuthServiceTestCases.TestCase testCase)
        {
            // Arrange
            _testSetup.SetupTokenGeneratorMock(_tokenGeneratorMock, testCase);

            // Act
            var result = await _userAuthService.LoginAsync(testCase.Email, testCase.Password);

            // Assert
            result.Should().NotBeNull();
            result?.Token.Should().Be(testCase.ExpectedToken);
            result?.RefreshToken.Should().Be(testCase.ExpectedRefreshToken);
        }

        [Theory]
        [MemberData(nameof(AuthServiceTestCases.ValidCredentialsTestCases), MemberType = typeof(AuthServiceTestCases))]
        public async Task RegisterAsync_ReturnsSuccessful_WhenValidInput(AuthServiceTestCases.TestCase testCase)
        {
            // Arrange
            var newUser = new User { Email = testCase.Email, Password = testCase.Password };
            _userRepositoryMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult<User>(null));
            var authService = new UserAuthService(_userRepositoryMock.Object, _passwordHasherMock.Object, _emailValidatorMock.Object, _passwordValidatorMock.Object, _tokenGeneratorMock.Object, _loggerMock.Object);

            // Act
            var result = await authService.RegisterAsync(newUser);

            // Assert
            result.Should().Be(UserRegistrationResult.Successful);
        }

        [Fact]
        public async Task LogoutAsync_ReturnsTrue()
        {
            // Arrange
            var authService = new UserAuthService(_userRepositoryMock.Object, _passwordHasherMock.Object, _emailValidatorMock.Object, _passwordValidatorMock.Object, _tokenGeneratorMock.Object, _loggerMock.Object);

            // Act
            var result = await authService.LogoutAsync("userId");

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteUserAsync_ReturnsTrue()
        {
            // Arrange
            var authService = new UserAuthService(_userRepositoryMock.Object, _passwordHasherMock.Object, _emailValidatorMock.Object, _passwordValidatorMock.Object, _tokenGeneratorMock.Object, _loggerMock.Object);

            // Act
            var result = await authService.DeleteUserAuthDetailsAsync("userId");

            // Assert
            result.Should().BeTrue();
        }
        
         [Fact]
        public async Task LoginAsync_ReturnsNull_WhenInvalidEmail()
        {
            // Arrange
            _emailValidatorMock.Setup(x => x.IsValid(It.IsAny<string>())).Returns(false);
            var authService = new UserAuthService(_userRepositoryMock.Object, _passwordHasherMock.Object, _emailValidatorMock.Object, _passwordValidatorMock.Object, _tokenGeneratorMock.Object, _loggerMock.Object);

            // Act
            var result = await authService.LoginAsync("invalidemail", "ValidPassword");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task LoginAsync_ReturnsNull_WhenInvalidPassword()
        {
            // Arrange
            _passwordHasherMock.Setup(x => x.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>())).Returns(false);
            var authService = new UserAuthService(_userRepositoryMock.Object, _passwordHasherMock.Object, _emailValidatorMock.Object, _passwordValidatorMock.Object, _tokenGeneratorMock.Object, _loggerMock.Object);

            // Act
            var result = await authService.LoginAsync("validemail@example.com", "InvalidPassword");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task RegisterAsync_ReturnsInvalidEmail_WhenInvalidEmail()
        {
            // Arrange
            _emailValidatorMock.Setup(x => x.IsValid(It.IsAny<string>())).Returns(false);
            var newUser = new User { Email = "invalidemail", Password = "ValidPassword" };
            var authService = new UserAuthService(_userRepositoryMock.Object, _passwordHasherMock.Object, _emailValidatorMock.Object, _passwordValidatorMock.Object, _tokenGeneratorMock.Object, _loggerMock.Object);

            // Act
            var result = await authService.RegisterAsync(newUser);

            // Assert
            result.Should().Be(UserRegistrationResult.InvalidEmail);
        }

        [Fact]
        public async Task RegisterAsync_ReturnsEmailAlreadyExists_WhenEmailAlreadyExists()
        {
            // Arrange
            _userRepositoryMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult(new User()));
            var newUser = new User { Email = "existingemail@example.com", Password = "ValidPassword" };
            var authService = new UserAuthService(_userRepositoryMock.Object, _passwordHasherMock.Object, _emailValidatorMock.Object, _passwordValidatorMock.Object, _tokenGeneratorMock.Object, _loggerMock.Object);

            // Act
            var result = await authService.RegisterAsync(newUser);

            // Assert
            result.Should().Be(UserRegistrationResult.EmailAlreadyExists);
        } 

        [Fact]
        public async Task RegisterAsync_ReturnsInvalidPassword_WhenInvalidPassword()
        {
            // Arrange
            _passwordValidatorMock.Setup(x => x.IsValid(It.IsAny<string>())).Returns(false);
            _userRepositoryMock.Setup(x => x.FindByEmailAsync("newuser@example.com")).Returns(Task.FromResult<User>(null)); // Add this line
    
            var newUser = new User { Email = "newuser@example.com", Password = "InvalidPassword" };
            var authService = new UserAuthService(_userRepositoryMock.Object, _passwordHasherMock.Object, _emailValidatorMock.Object, _passwordValidatorMock.Object, _tokenGeneratorMock.Object, _loggerMock.Object);

            // Act
            var result = await authService.RegisterAsync(newUser);

            // Assert
            result.Should().Be(UserRegistrationResult.InvalidPassword);
        }
    }
}
