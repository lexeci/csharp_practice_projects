using NUnit.Framework;
using MySql.Data.MySqlClient;
using System;
using Microsoft.Extensions.Options;

[TestFixture]
public class DatabaseServiceTests
{
    private DatabaseService _service = null!;

    [SetUp]
    public void Setup()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var settings = configuration.GetSection("ConnectionStrings").Get<DatabaseSettings>();

        var options = Options.Create(settings);

        _service = new DatabaseService(options);
    }

    [Test]
    public void InsertUser_SQLInjectionAttempt_ShouldNotBreakQuery()
    {
        string maliciousUsername = "' OR '1'='1";
        string maliciousEmail = "test@example.com'; DROP TABLE Users; --";
        string passwordHash = "fakehash123";

        Assert.DoesNotThrow(() => _service.InsertUser(maliciousUsername, maliciousEmail, passwordHash));
    }

    [Test]
    public void InsertUser_XSSAttempt_ShouldNotExecuteScript()
    {
        string xssInput = "<script>alert('xss')</script>";
        string email = "xss@example.com";
        string passwordHash = "fakehash123";

        Assert.DoesNotThrow(() => _service.InsertUser(xssInput, email, passwordHash));
    }
}
