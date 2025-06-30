using System.ComponentModel.DataAnnotations;

public class RegisterModel
{
    // Username required, length between 3 and 50 characters
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public required string UserName { get; set; }

    // Email address is required and must be a valid email format
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    // Password is required, minimum length 6 and maximum 100 characters
    [Required]
    [StringLength(100, MinimumLength = 6)]
    public required string Password { get; set; }
}

public class LoginModel
{
    // Username is required for login
    [Required]
    public required string UserName { get; set; }

    // Password is required for login
    [Required]
    public required string Password { get; set; }
}

public class AssignRoleModel
{
    // Email of the user to whom the role will be assigned, required field
    [Required]
    public required string Email { get; set; }

    // Role to assign to the user, required field
    [Required]
    public required string Role { get; set; }
}
