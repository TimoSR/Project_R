using System.Linq.Expressions;
using Application.AppServices.V1;
using Domain.DomainModels;
using Domain.IRepositories;
using Infrastructure.Utilities._Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace Unit_Tests.Authentication.V1.AuthServiceSetup;

public class AuthServiceTestSetup
{
    private const string ExpectedToken = "SampleToken";
    private const string ExpectedRefreshToken = "SampleRefreshToken";

    public void SetupDefaultMocks(
        Mock<IEmailValidator> emailValidatorMock,
        Mock<IPasswordValidator> passwordValidatorMock,
        Mock<IPasswordHasher> passwordHasherMock,
        Mock<ITokenHandler> tokenGeneratorMock,
        Mock<IUserRepository> userRepositoryMock)
    {
        emailValidatorMock.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);
        passwordValidatorMock.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);
        passwordHasherMock.Setup(x => x.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>())).Returns(true);
        tokenGeneratorMock.Setup(x => x.GenerateToken(It.IsAny<string>())).Returns(ExpectedToken);
        userRepositoryMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(new User());
        tokenGeneratorMock.Setup(t => t.GenerateRefreshToken()).Returns(ExpectedRefreshToken);
    }

    public void SetupTokenGeneratorMock(Mock<ITokenHandler> tokenGeneratorMock, AuthServiceTestCases.TestCase testCase)
    {
        tokenGeneratorMock.Setup(x => x.GenerateToken(It.Is<string>(s => s == testCase.Email))).Returns(testCase.ExpectedToken);
        tokenGeneratorMock.Setup(t => t.GenerateRefreshToken()).Returns(testCase.ExpectedRefreshToken);
    }
    
    public void SetupInvalidMocks(
        Mock<IEmailValidator> emailValidatorMock,
        Mock<IPasswordValidator> passwordValidatorMock,
        Mock<IPasswordHasher> passwordHasherMock,
        Mock<IUserRepository> userRepositoryMock)
    {
        emailValidatorMock.Setup(x => x.IsValid(It.IsAny<string>())).Returns(false);
        passwordValidatorMock.Setup(x => x.IsValid(It.IsAny<string>())).Returns(false);
        passwordHasherMock.Setup(x => x.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>())).Returns(false);
        userRepositoryMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null);
    }


    public void SetupUserRepositoryMock(Mock<IUserRepository> userRepositoryMock, User user)
    {
        userRepositoryMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
    }

    public void SetupCustomMock<T, TResult>(Mock<T> mock, Expression<Func<T, TResult>> expression, TResult returnValue) where T : class
    {
        mock.Setup(expression).Returns(returnValue);
    }

    public AuthService InitializeAuthService(
        Mock<IUserRepository> userRepositoryMock,
        Mock<IPasswordHasher> passwordHasherMock,
        Mock<IEmailValidator> emailValidatorMock,
        Mock<IPasswordValidator> passwordValidatorMock,
        Mock<ITokenHandler> tokenGeneratorMock,
        Mock<ILogger<AuthService>> loggerMock)
    {
        return new AuthService(
            userRepositoryMock.Object,
            passwordHasherMock.Object,
            emailValidatorMock.Object,
            passwordValidatorMock.Object,
            tokenGeneratorMock.Object,
            loggerMock.Object);
    }
    
    
}