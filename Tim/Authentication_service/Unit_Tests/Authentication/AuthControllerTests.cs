using Application.AppServices._Interfaces;
using Application.Controllers.REST;
using Application.DataTransferObjects.Auth;
using Application.DataTransferObjects.UserManagement;
using Domain.DomainModels;
using Domain.DomainModels.Enums;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Unit_Tests.Authentication;

 public class AuthControllerTests
{
    private readonly AuthController _authController;
    private readonly Mock<IAuthService> _authServiceMock = new Mock<IAuthService>();

    public AuthControllerTests()
    {
        _authController = new AuthController(_authServiceMock.Object);
    }

    [Fact]
    public async Task Register_ShouldReturnOk_WhenRegistrationIsSuccessful()
    {
        // Arrange
        _authServiceMock.Setup(service => service.RegisterAsync(It.IsAny<User>()))
            .ReturnsAsync(UserRegistrationResult.Successful);

        // Act
        var result = await _authController.Register(new UserRegisterDto { Email = "test@example.com", Password = "Password123!" });

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Login_ShouldReturnOk_WhenLoginIsSuccessful()
    {
        // Arrange
        _authServiceMock.Setup(service => service.LoginAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((Token: "ValidToken", RefreshToken: "ValidRefreshToken"));

        // Act
        var result = await _authController.Login(new LoginRequestDto { Email = "test@example.com", Password = "Password123!" });

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;

        // Further assertions to check if the response contains Token and RefreshToken
        Assert.NotNull(okResult?.Value);
        var loginResponse = okResult?.Value as LoginResponseDto;
        Assert.NotNull(loginResponse);
        Assert.Equal("ValidToken", loginResponse?.Token);
        Assert.Equal("ValidRefreshToken", loginResponse?.RefreshToken);  // Add RefreshToken to LoginResponseDto if it's not already there
    }

    [Fact]
    public async Task Logout_ShouldReturnOk_WhenLogoutIsSuccessful()
    {
        // Arrange
        _authServiceMock.Setup(service => service.LogoutAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        // Act
        var result = await _authController.Logout(new LogoutRequestDto { UserId = "UserId" });

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task DeleteUser_ShouldReturnOk_WhenDeletionIsSuccessful()
    {
        // Arrange
        _authServiceMock.Setup(service => service.DeleteUserAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        // Act
        var result = await _authController.DeleteUser(new DeleteRequestDto { UserId = "UserId" });

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
}