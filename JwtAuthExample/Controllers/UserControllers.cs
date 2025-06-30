using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private static readonly Dictionary<string, string> Users = new()
    {
        { "user1", "password1" },
        { "admin", "password2" }
    };

    private readonly string secretKey = "SuperSecretKeyForJwtTokenAuthorization123456789";

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (!Users.TryGetValue(request.Username, out var password) || password != request.Password)
            return Unauthorized("Invalid username or password.");

        var token = GenerateJwtToken(request.Username);

        return Ok(new { Token = token });
    }

}

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}