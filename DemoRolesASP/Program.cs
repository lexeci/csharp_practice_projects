using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<IdentityDbContext>(options => options.UseInMemoryDatabase("AuthDemoDb"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<IdentityDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        if (context.Request.Path.StartsWithSegments("/") && context.Response.StatusCode == StatusCodes.Status200OK)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        }
        else
        {
            context.Response.Redirect(context.RedirectUri);
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthentication();
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Admin", policy => policy.RequireRole("Admin"))
    .AddPolicy("ItDepartment", policy => policy.RequireClaim("Department", "IT"));

var app = builder.Build();

app.MapGet("/", () => "I am root!");
app.MapGet("/admin-only", () => "Admin access only").RequireAuthorization("Admin");
app.MapGet("/user-claim-check", () => "Access granted to IT department").RequireAuthorization("ITDepartment");
app.MapGet("/login", () => "This is the login route");
var roles = new[] { "Admin", "User" };

app.MapPost("/create-role", async (RoleManager<IdentityRole> roleManager) =>
{
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    return Results.Ok("Roles created successfully");
});

app.MapPost("/assign-roles", async (UserManager<IdentityUser> userManager) =>
{
    var user = new IdentityUser { UserName = "test@usermail.com", Email = "test@usermail.com" };

    await userManager.CreateAsync(user, "Test@1234");
    await userManager.AddToRoleAsync(user, "Admin");

    var isInRole = await userManager.IsInRoleAsync(user, "Admin");
    return isInRole ? Results.Ok() : Results.BadRequest();
});

app.MapPost("/login", async (SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager) => {

    var user = await userManager.FindByEmailAsync("test@usermail.com");

    if(user == null) {
        Results.NotFound("User not found");
    }

    await signInManager.SignInAsync(user, isPersistent: false);
    return Results.Ok("User sign in!");
});

app.MapPost("/add-claim", async (UserManager<IdentityUser> userManager) => {
    var user = await userManager.FindByEmailAsync("test@usermail.com");

    if (user == null) {
        return Results.NotFound("User not found");
    }

    await userManager.AddClaimAsync(user, new Claim("Department", "IT"));

    var claims = await userManager.GetClaimsAsync(user);
    var hasITClaim = claims.Any(C => C.Type == "Department" && C.Value == "IT");

    return hasITClaim ? Results.Ok() : Results.BadRequest();
});

app.Run();