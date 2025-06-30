using LogiTrack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _roleManager = roleManager;
    }

    // GET: /api/auth/test
    // Checks if the current user is authorized and returns their user ID
    [Authorize]
    [HttpGet("test")]
    public IActionResult Test()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Ok($"You are authorized! User ID: {userId}");
    }

    // GET: /api/auth/roles
    // Returns the roles of the currently authenticated user from their claims
    [Authorize]
    [HttpGet("roles")]
    public IActionResult GetUserRoles()
    {
        var roles = User.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();

        return Ok(new { Roles = roles });
    }

    // POST: /api/auth/register
    // Registers a new user with the "User" role by default
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Check if email already exists
        if (await _userManager.FindByEmailAsync(model.Email) != null)
            return BadRequest("Email already in use");

        // Check if username already taken
        if (await _userManager.FindByNameAsync(model.UserName) != null)
            return BadRequest("Username already taken");

        var user = new ApplicationUser
        {
            UserName = model.UserName,
            Email = model.Email,
        };

        // Create the user with the provided password
        var result = await _userManager.CreateAsync(user, model.Password);

        // Assign "User" role if not already assigned
        if (!await _userManager.IsInRoleAsync(user, "User"))
        {
            result = await _userManager.AddToRoleAsync(user, "User");

            if (!result.Succeeded)
                return BadRequest(result.Errors);
        }

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(new { Message = "User registered successfully" });
    }

    // POST: /api/auth/login
    // Authenticates a user and returns a JWT token if successful
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.FindByNameAsync(model.UserName);
        if (user == null)
            return Unauthorized("Invalid username or password");

        // Verify password
        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        if (!result.Succeeded)
            return Unauthorized("Invalid username or password");

        // Generate JWT token with claims and roles
        var token = await GenerateJwtToken(user);

        return Ok(new { Token = token });
    }

    // POST: /api/auth/create-role
    // Creates a new role; restricted to Admin users only
    [HttpPost("create-role")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateRole([FromBody] string roleName)
    {
        if (await _roleManager.RoleExistsAsync(roleName))
            return BadRequest("Role already exists");

        var result = await _roleManager.CreateAsync(new IdentityRole(roleName));

        if (result.Succeeded)
            return Ok($"Role {roleName} created successfully");

        return BadRequest(result.Errors);
    }

    // POST: /api/auth/assign-role
    // Assigns an existing role to a user; restricted to Admin users only
    [HttpPost("assign-role")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
            return NotFound("User not found");

        // Assign role if user does not already have it
        if (!await _userManager.IsInRoleAsync(user, model.Role))
        {
            var result = await _userManager.AddToRoleAsync(user, model.Role);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
        }

        // Generate a new JWT token including the updated roles
        var token = await GenerateJwtToken(user);
        return Ok(new
        {
            Message = $"Role '{model.Role}' assigned to user '{model.Email}'.",
            Token = token
        });
    }

    // Private helper method to generate JWT token for authenticated users,
    // including their roles as claims
    private async Task<string> GenerateJwtToken(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

        // Add role claims
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var jwtKey = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("JWT key not configured.");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
