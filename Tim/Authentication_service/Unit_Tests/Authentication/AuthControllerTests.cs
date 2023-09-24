using Application.AppServices._Interfaces;
using Application.Controllers.REST;
using Application.DataTransferObjects.Auth;
using Application.DataTransferObjects.UserManagement;
using Domain.DomainModels;
using Domain.DomainModels.Enums;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Unit_Tests;

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
    public async Task Authenticate_ShouldReturnOk_WhenAuthenticationIsSuccessful()
    {
        // Arrange
        _authServiceMock.Setup(service => service.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync("ValidToken");

        // Act
        var result = await _authController.Authenticate(new AuthRequestDto { Email = "test@example.com", Password = "Password123!" });

        // Assert
        Assert.IsType<OkObjectResult>(result);
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