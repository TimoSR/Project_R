using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Infrastructure.Utilities;
using Infrastructure.Utilities._Interfaces;
using Infrastructure.Utilities.TokenGenerator;
using Moq;

namespace Unit_Tests.UtilityTests;

public class TokenGeneratorTests
{
    private readonly TokenGenerator _tokenGenerator;
    private readonly IConfiguration _configuration;
    private readonly string _sampleKey;

    public TokenGeneratorTests()
    {        
        _sampleKey = Convert.ToBase64String(Encoding.UTF8.GetBytes("NzQ5OTU4YzEtNmFhZi00YmFkLWIxNWMtYjY4YzQwN2I3ZDA5"));
        _configuration = new Configuration()
        {
            JwtKey = _sampleKey,
        };
        _tokenGenerator = new TokenGenerator(_configuration);
    }

    [Fact]
    public void GenerateToken_WhenCalled_ShouldNotReturnNull()
    {
        // Arrange
        var userId = "test-user-id";

        // Act
        var result = _tokenGenerator.GenerateToken(userId);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GenerateToken_WhenCalled_ShouldContainUserIdClaim()
    {
        // Arrange
        var userId = "test-user-id";

        // Act
        var tokenString = _tokenGenerator.GenerateToken(userId);
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
        var tokenString = _tokenGenerator.GenerateToken(userId);
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.ReadJwtToken(tokenString);

        // Assert
        Assert.True(token.ValidTo > DateTime.UtcNow);
        Assert.True(token.ValidTo < DateTime.UtcNow.AddHours(2)); // Assuming token expires in 1 hour
    }
}