using Domain.DomainModels;
using Domain.IRepositories;
using Infrastructure.Utilities._Interfaces;
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
}