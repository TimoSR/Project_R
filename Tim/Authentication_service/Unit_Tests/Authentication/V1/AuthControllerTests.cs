using Application.AppServices.V1._Interfaces;
using Application.Controllers.REST.V1;
using Application.DataTransferObjects.Auth;
using Application.DataTransferObjects.UserManagement;
using Domain.DomainModels;
using Domain.DomainModels.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Unit_Tests.Authentication.V1
{
    public class AuthControllerTests
    {
        private readonly AuthController _authController;
        private readonly Mock<IAuthService> _authServiceMock = new Mock<IAuthService>();

        public AuthControllerTests()
        {
            _authController = new AuthController(_authServiceMock.Object);
        }
        
        [Fact]
        public void CheckTokenValidity_ShouldReturnOk()
        {
            // Act
            var result = _authController.CheckTokenValidity() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
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
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
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
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
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
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
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
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
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
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }
    }
}
