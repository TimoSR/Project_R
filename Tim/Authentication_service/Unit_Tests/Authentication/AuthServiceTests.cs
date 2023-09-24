using Application.AppServices;
using Domain.DomainModels;
using Domain.DomainModels.Enums;
using Domain.IRepositories;
using Infrastructure.Utilities._Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace Unit_Tests.Authentication
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
        private readonly Mock<IPasswordHasher> _passwordHasherMock = new Mock<IPasswordHasher>();
        private readonly Mock<IEmailValidator> _emailValidatorMock = new Mock<IEmailValidator>();
        private readonly Mock<IPasswordValidator> _passwordValidatorMock = new Mock<IPasswordValidator>();
        private readonly Mock<ITokenHandler> _tokenGeneratorMock = new Mock<ITokenHandler>();
        private readonly Mock<ILogger<AuthService>> _loggerMock = new Mock<ILogger<AuthService>>();
        private readonly string expectedToken;
        private readonly string expectedRefreshToken;

        public AuthServiceTests()
        {
            
            // Expected values
            expectedToken = "SampleToken";
            expectedRefreshToken = "SampleRefreshToken";
            
            // Setup default mock behaviors, can be overridden in specific test cases
            _emailValidatorMock.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);
            _passwordValidatorMock.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);
            _passwordHasherMock.Setup(x => x.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>())).Returns(true);
            _tokenGeneratorMock.Setup(x => x.GenerateToken(It.IsAny<string>())).Returns("SampleToken");
            _userRepositoryMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult(new User()));
            _tokenGeneratorMock.Setup(t => t.GenerateToken(It.IsAny<string>()))
                .Returns(expectedToken);
    
            _tokenGeneratorMock.Setup(t => t.GenerateRefreshToken())
                .Returns(expectedRefreshToken);
            
        }

        [Fact]
        public async Task LoginAsync_ReturnsToken_WhenValidCredentials()
        {
            // Arrange
            var authService = new AuthService(_userRepositoryMock.Object, _passwordHasherMock.Object, _emailValidatorMock.Object, _passwordValidatorMock.Object, _tokenGeneratorMock.Object, _loggerMock.Object);

            // Act
            var result = await authService.LoginAsync("validemail@gmail.com", "ValidPassword1!");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedToken, result?.Token);
            Assert.Equal(expectedRefreshToken, result?.RefreshToken);
        }

        [Fact]
        public async Task RegisterAsync_ReturnsSuccessful_WhenValidInput()
        {
            // Arrange
            var newUser = new User { Email = "newuser@example.com", Password = "ValidPassword" };
            _userRepositoryMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult<User>(null));
            var authService = new AuthService(_userRepositoryMock.Object, _passwordHasherMock.Object, _emailValidatorMock.Object, _passwordValidatorMock.Object, _tokenGeneratorMock.Object, _loggerMock.Object);

            // Act
            var result = await authService.RegisterAsync(newUser);

            // Assert
            Assert.Equal(UserRegistrationResult.Successful, result);
        }

        [Fact]
        public async Task LogoutAsync_ReturnsTrue()
        {
            // Arrange
            var authService = new AuthService(_userRepositoryMock.Object, _passwordHasherMock.Object, _emailValidatorMock.Object, _passwordValidatorMock.Object, _tokenGeneratorMock.Object, _loggerMock.Object);

            // Act
            var result = await authService.LogoutAsync("userId");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteUserAsync_ReturnsTrue()
        {
            // Arrange
            var authService = new AuthService(_userRepositoryMock.Object, _passwordHasherMock.Object, _emailValidatorMock.Object, _passwordValidatorMock.Object, _tokenGeneratorMock.Object, _loggerMock.Object);

            // Act
            var result = await authService.DeleteUserAsync("userId");

            // Assert
            Assert.True(result);
        }
        
         [Fact]
        public async Task LoginAsync_ReturnsNull_WhenInvalidEmail()
        {
            // Arrange
            _emailValidatorMock.Setup(x => x.IsValid(It.IsAny<string>())).Returns(false);
            var authService = new AuthService(_userRepositoryMock.Object, _passwordHasherMock.Object, _emailValidatorMock.Object, _passwordValidatorMock.Object, _tokenGeneratorMock.Object, _loggerMock.Object);

            // Act
            var result = await authService.LoginAsync("invalidemail", "ValidPassword");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task LoginAsync_ReturnsNull_WhenInvalidPassword()
        {
            // Arrange
            _passwordHasherMock.Setup(x => x.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>())).Returns(false);
            var authService = new AuthService(_userRepositoryMock.Object, _passwordHasherMock.Object, _emailValidatorMock.Object, _passwordValidatorMock.Object, _tokenGeneratorMock.Object, _loggerMock.Object);

            // Act
            var result = await authService.LoginAsync("validemail@example.com", "InvalidPassword");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task RegisterAsync_ReturnsInvalidEmail_WhenInvalidEmail()
        {
            // Arrange
            _emailValidatorMock.Setup(x => x.IsValid(It.IsAny<string>())).Returns(false);
            var newUser = new User { Email = "invalidemail", Password = "ValidPassword" };
            var authService = new AuthService(_userRepositoryMock.Object, _passwordHasherMock.Object, _emailValidatorMock.Object, _passwordValidatorMock.Object, _tokenGeneratorMock.Object, _loggerMock.Object);

            // Act
            var result = await authService.RegisterAsync(newUser);

            // Assert
            Assert.Equal(UserRegistrationResult.InvalidEmail, result);
        }

        [Fact]
        public async Task RegisterAsync_ReturnsEmailAlreadyExists_WhenEmailAlreadyExists()
        {
            // Arrange
            _userRepositoryMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult(new User()));
            var newUser = new User { Email = "existingemail@example.com", Password = "ValidPassword" };
            var authService = new AuthService(_userRepositoryMock.Object, _passwordHasherMock.Object, _emailValidatorMock.Object, _passwordValidatorMock.Object, _tokenGeneratorMock.Object, _loggerMock.Object);

            // Act
            var result = await authService.RegisterAsync(newUser);

            // Assert
            Assert.Equal(UserRegistrationResult.EmailAlreadyExists, result);
        }

        [Fact]
        public async Task RegisterAsync_ReturnsInvalidPassword_WhenInvalidPassword()
        {
            // Arrange
            _passwordValidatorMock.Setup(x => x.IsValid(It.IsAny<string>())).Returns(false);
            _userRepositoryMock.Setup(x => x.FindByEmailAsync("newuser@example.com")).Returns(Task.FromResult<User>(null)); // Add this line
    
            var newUser = new User { Email = "newuser@example.com", Password = "InvalidPassword" };
            var authService = new AuthService(_userRepositoryMock.Object, _passwordHasherMock.Object, _emailValidatorMock.Object, _passwordValidatorMock.Object, _tokenGeneratorMock.Object, _loggerMock.Object);

            // Act
            var result = await authService.RegisterAsync(newUser);

            // Assert
            Assert.Equal(UserRegistrationResult.InvalidPassword, result);
        }

    }
}
