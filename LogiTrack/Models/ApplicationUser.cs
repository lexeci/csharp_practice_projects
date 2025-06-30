using Microsoft.AspNetCore.Identity;

namespace LogiTrack.Models // або LogiTrack.Identity, залежно від організації проекту
{
    // Represents the application user extending ASP.NET Core Identity's built-in user class.
    // Add custom properties for user profile here as needed.
    public class ApplicationUser : IdentityUser
    {
        // Example: Additional profile data can be added here
        // public string? FullName { get; set; }
        // public DateTime? DateOfBirth { get; set; }
    }
}
