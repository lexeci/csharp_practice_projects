using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using NUnit.Framework;

[TestFixture]
public class AuthenticationServiceTests
{
    private AuthenticationService _authService = null!;

    [SetUp]
    public void Setup()
    {
        var configuration = new ConfigurationBuilder()
       .SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json")
       .Build();

        var settings = configuration.GetSection("ConnectionStrings").Get<DatabaseSettings>();

        var options = Options.Create(settings);

        _authService = new AuthenticationService(options);

        _authService.ClearTest();
    }

    [Test]
    public void Register_And_Authenticate_ValidUser_ShouldPass()
    {
        var username = "testUser_" + Guid.NewGuid().ToString("N").Substring(0, 8);
        var password = "securePassword123";
        var email = username + "@test.com";
        var role = "user";

        _authService.Register(username, email, password, role);

        var result = _authService.Authenticate(username, password);
        Assert.That(result, Is.True);
    }

    [Test]
    public void Authenticate_InvalidPassword_ShouldFail()
    {
        string username = "testUser2";
        _authService.Register(username, "user2@domain.com", "password123", "user");

        bool result = _authService.Authenticate(username, "wrongPassword");
        Assert.That(result, Is.False);
    }

    [Test]
    public void GetUserRole_ValidUser_ShouldReturnCorrectRole()
    {
        string username = "adminUser";
        _authService.Register(username, "admin@domain.com", "adminpass", "admin");

        string role = _authService.GetUserRole(username);
        Assert.That(role, Is.EqualTo("admin"));
    }
}
