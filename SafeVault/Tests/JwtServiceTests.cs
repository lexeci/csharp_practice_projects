using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

[TestFixture]
public class JwtServiceTests
{
    private JwtService _jwtService = null!;

    [SetUp]
    public void Setup()
    {
        var jwtSettings = new JwtSettings { Key = "test-key-123456789-test-key-123456789-test-key-123456789", Issuer = "test-issuer" };
        var options = Options.Create(jwtSettings);
        _jwtService = new JwtService(options);
    }

    [Test]
    public void GenerateToken_ShouldContainCorrectClaims()
    {
        // Arrange
        var username = "testuser";
        var role = "admin";

        // Act
        var token = _jwtService.GenerateToken(username, role);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        Assert.That(nameClaim, Is.EqualTo(username));
        Assert.That(roleClaim, Is.EqualTo(role));
    }

    [Test]
    public void GenerateToken_ShouldBeValid()
    {
        var username = "testuser";
        var role = "admin";
        var token = _jwtService.GenerateToken(username, role);

        var handler = new JwtSecurityTokenHandler();
        var validationParams = _jwtService.GetValidationParameters();

        // Assert no exception is thrown and validatedToken is not null
        JwtSecurityToken? validatedJwtToken = null;

        Assert.DoesNotThrow(() =>
        {
            handler.ValidateToken(token, validationParams, out var validatedToken);
            validatedJwtToken = validatedToken as JwtSecurityToken;
        });

        Assert.That(validatedJwtToken, Is.Not.Null);
        Assert.That(validatedJwtToken, Is.InstanceOf<JwtSecurityToken>());
    }
}
