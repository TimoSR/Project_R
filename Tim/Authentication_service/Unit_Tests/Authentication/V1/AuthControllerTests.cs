using Application.Controllers.REST.V1;
using Application.DTO.Auth;
using Application.DTO.UserManagement;
using Domain.DomainModels;
using Domain.DomainModels.Enums;
using Domain.UserManagement.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Unit_Tests.Authentication.V1.AuthServiceSetup;

namespace Unit_Tests.Authentication.V1
{
    public class AuthControllerTests
    {
        private readonly AuthController _authController;
        private readonly Mock<IAuthServiceV1> _authServiceMock;

        public AuthControllerTests()
        {
            _authServiceMock = new Mock<IAuthServiceV1>();
            _authController = new AuthController(_authServiceMock.Object);
        }

        [Fact]
        public void CheckTokenValidity_ShouldReturnOk()
        {
            // Arrange (Already arranged in constructor)

            // Act
            var result = _authController.CheckTokenValidity() as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task RefreshToken_ShouldReturnOk()
        {
            // Arrange
            _authServiceMock.Setup(service => service.RefreshTokenAsync(It.IsAny<string>()))
                .ReturnsAsync((NewToken: "NewValidToken", NewRefreshToken: "NewValidRefreshToken"));

            // Act
            var result = await _authController.RefreshToken(new AuthRequestDto { RefreshToken = "ValidRefreshToken" }) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task Register_ShouldReturnOk_WhenRegistrationIsSuccessful()
        {
            // Arrange
            _authServiceMock.Setup(service => service.RegisterAsync(It.IsAny<User>()))
                .ReturnsAsync(UserRegistrationResult.Successful);

            // Act
            var result = await _authController.Register(new UserRegisterDto { Email = "test@example.com", Password = "Password123!" }) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task Login_ShouldReturnOk_WhenLoginIsSuccessful()
        {
            // Arrange
            _authServiceMock.Setup(service => service.LoginAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((Token: "ValidToken", RefreshToken: "ValidRefreshToken"));

            // Act
            var result = await _authController.Login(new LoginRequestDto { Email = "test@example.com", Password = "Password123!" }) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task Logout_ShouldReturnOk_WhenLogoutIsSuccessful()
        {
            // Arrange
            _authServiceMock.Setup(service => service.LogoutAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            var result = await _authController.Logout(new LogoutRequestDto { UserId = "UserId" }) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnOk_WhenDeletionIsSuccessful()
        {
            // Arrange
            _authServiceMock.Setup(service => service.DeleteUserAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            var result = await _authController.DeleteUser(new DeleteRequestDto { UserId = "UserId" }) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
        
        [Theory]
        [MemberData(nameof(AuthServiceTestCases.ValidCredentialsTestCases), MemberType = typeof(AuthServiceTestCases))]
        public async Task Login_ShouldReturnOk_WithValidCredentials(AuthServiceTestCases.TestCase testCase)
        {
            // Arrange
            _authServiceMock.Setup(service => service.LoginAsync(testCase.Email, testCase.Password))
                .ReturnsAsync((Token: testCase.ExpectedToken, RefreshToken: testCase.ExpectedRefreshToken));

            // Act
            var result = await _authController.Login(new LoginRequestDto { Email = testCase.Email, Password = testCase.Password }) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
    }
}
