using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Infrastructure.Utilities;
using Infrastructure.Utilities._Interfaces;
using Infrastructure.Utilities.TokenGenerator;
using Microsoft.Extensions.Logging;
using Moq;

namespace Unit_Tests.UtilityTests;

public class TokenGeneratorTests
{
    private readonly TokenHandler _tokenHandler;
    private readonly IConfiguration _configuration;
    private readonly string _sampleKey;
    private readonly string _sampleIssuer;
    private readonly string _sampleAudience;

    public TokenGeneratorTests()
    {        
        _sampleKey = Convert.ToBase64String(Encoding.UTF8.GetBytes("NzQ5OTU4YzEtNmFhZi00YmFkLWIxNWMtYjY4YzQwN2I3ZDA5"));
        _sampleIssuer = "SampleIssuer";
        _sampleAudience = "SampleAudience";
        
        var configMock = new Configuration()
        {
            JwtKey = _sampleKey,
            JwtIssuer = _sampleIssuer,
            JwtAudience = _sampleAudience
        };
        
        var loggerMock = new Mock<ILogger<TokenHandler>>();
        
        _tokenHandler = new TokenHandler(configMock, loggerMock.Object);
    }

    [Fact]
    public void GenerateToken_WhenCalled_ShouldNotReturnNull()
    {
        // Arrange
        var userId = "test-user-id";

        // Act
        var result = _tokenHandler.GenerateToken(userId);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GenerateToken_WhenCalled_ShouldContainUserIdClaim()
    {
        // Arrange
        var userId = "test-user-id";

        // Act
        var tokenString = _tokenHandler.GenerateToken(userId);
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.ReadJwtToken(tokenString);

        // Assert
        var claim = token.Claims.First(c => c.Type == "id");
        Assert.Equal(userId, claim.Value);
    }

    [Fact]
    public void GenerateToken_WhenCalled_ShouldHaveValidExpiration()
    {
        // Arrange
        var userId = "test-user-id";

        // Act
        var tokenString = _tokenHandler.GenerateToken(userId);
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.ReadJwtToken(tokenString);

        // Assert
        Assert.True(token.ValidTo > DateTime.UtcNow);
        Assert.True(token.ValidTo < DateTime.UtcNow.AddHours(2));  // Assuming token expires in 1 hour
    }

    [Fact]
    public void GenerateToken_WhenCalled_ShouldHaveValidIssuer()
    {
        // Arrange
        var userId = "test-user-id";

        // Act
        var tokenString = _tokenHandler.GenerateToken(userId);
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.ReadJwtToken(tokenString);

        // Assert
        Assert.Equal(_sampleIssuer, token.Issuer);
    }

    [Fact]
    public void GenerateToken_WhenCalled_ShouldHaveValidAudience()
    {
        // Arrange
        var userId = "test-user-id";

        // Act
        var tokenString = _tokenHandler.GenerateToken(userId);
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.ReadJwtToken(tokenString);

        // Assert
        Assert.True(token.Audiences.Contains(_sampleAudience));
    }
}