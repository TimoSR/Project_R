using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Infrastructure.Utilities;
using Infrastructure.Utilities._Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Moq;
using TokenHandler = Infrastructure.Utilities.Token.TokenHandler;

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
    
    private string GenerateExpiredToken()
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("NzQ5OTU4YzEtNmFhZi00YmFkLWIxNWMtYjY4YzQwN2I3ZDA5"); // Invalid key to generate invalid token
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", "1") }),
            Expires = DateTime.UtcNow.AddMinutes(-1), // Expired token
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
    }

    [Fact]
    public void GenerateToken_WhenCalled_ShouldNotReturnNull()
    {
        // Arrange
        var userId = "test-user-id";

        // Act
        var result = _tokenHandler.GenerateJwtToken(userId);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GenerateToken_WhenCalled_ShouldContainUserIdClaim()
    {
        // Arrange
        var userId = "test-user-id";

        // Act
        var tokenString = _tokenHandler.GenerateJwtToken(userId);
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
        var tokenString = _tokenHandler.GenerateJwtToken(userId);
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.ReadJwtToken(tokenString);

        // Assert
        Assert.True(token.ValidTo > DateTime.UtcNow);
        Assert.True(token.ValidTo < DateTime.UtcNow.AddHours(7));  // Assuming token expires in 6 hours
    }


    [Fact]
    public void GenerateToken_WhenCalled_ShouldHaveValidIssuer()
    {
        // Arrange
        var userId = "test-user-id";

        // Act
        var tokenString = _tokenHandler.GenerateJwtToken(userId);
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
        var tokenString = _tokenHandler.GenerateJwtToken(userId);
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.ReadJwtToken(tokenString);

        // Assert
        Assert.True(token.Audiences.Contains(_sampleAudience));
    }

    [Fact]
    public void DecodeToken_WhenExceptionThrown_ShouldThrowException()
    {
        // Arrange
        var token = "invalid_token";
    
        // Act
        Action action = () => _tokenHandler.DecodeJwtToken(token);
    
        // Assert
        Assert.Throws<SecurityTokenMalformedException>(action);
    }

}