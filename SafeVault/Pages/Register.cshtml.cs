using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

public class RegisterModel : PageModel
{
    [BindProperty, Required]
    public string Username { get; set; } = string.Empty;
    [BindProperty, Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    [BindProperty, Required, DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
    [BindProperty, Required]

    public string Role { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;


    public void OnPost()
    {
        if (!ModelState.IsValid)
        {
            Message = "Please correct the errors.";
            return;
        }

        var authService = new AuthenticationService();
        authService.Register(Username, Email, Password, Role);
        Message = "User registered successfully!";
    }
}
