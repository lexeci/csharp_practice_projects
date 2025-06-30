using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

public class LoginModel : PageModel
{
    private readonly JwtService _jwtService;

    public LoginModel(JwtService jwtService)
    {
        _jwtService = jwtService;
    }

    [BindProperty, Required]
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    public void OnPost()
    {
        var auth = new AuthenticationService();
        if (!auth.Authenticate(Username, Password))
        {
            Message = "Invalid username or password.";
            return;
        }

        var role = auth.GetUserRole(Username);
        Token = _jwtService.GenerateToken(Username, role);

        Response.Cookies.Append("jwt", Token, new CookieOptions { HttpOnly = true });

        Message = "Login successful!";
    }
}
