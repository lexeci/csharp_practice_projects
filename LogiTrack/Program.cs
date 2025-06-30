using System.Text.Json.Serialization;
using LogiTrack;
using LogiTrack.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configure and add DbContext with SQLite provider and database file "logitrack.db"
builder.Services.AddDbContext<LogiTrackContext>(options =>
    options.UseSqlite("Data Source=logitrack.db"));

// Configure Identity services with password requirements and add EF stores
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
.AddEntityFrameworkStores<LogiTrackContext>()
.AddDefaultTokenProviders();

// Configure authentication to use JWT Bearer tokens
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Retrieve JWT secret key from configuration, throw if missing
    var jwtKey = builder.Configuration["Jwt:Key"]
        ?? throw new InvalidOperationException("JWT key not configured.");

    // Setup JWT token validation parameters
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// Add support for controllers with JSON options to prevent reference cycles during serialization
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Add Swagger/OpenAPI support for automatic API documentation and testing
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Define example schema for InventoryItem in Swagger UI
    c.MapType<InventoryItem>(() => new OpenApiSchema
    {
        Type = "object",
        Properties = new Dictionary<string, OpenApiSchema>
        {
            ["itemId"] = new OpenApiSchema { Type = "integer", Example = new OpenApiInteger(1) },
            ["name"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("Monitor Dell") },
            ["quantity"] = new OpenApiSchema { Type = "integer", Example = new OpenApiInteger(5) },
            ["location"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("Warehouse A") },
            ["orderId"] = new OpenApiSchema { Type = "integer", Example = new OpenApiInteger(1) }
        }
    });

    // Define example schema for Order, including nested InventoryItem array
    c.MapType<Order>(() => new OpenApiSchema
    {
        Type = "object",
        Properties = new Dictionary<string, OpenApiSchema>
        {
            ["orderId"] = new OpenApiSchema { Type = "integer", Example = new OpenApiInteger(1) },
            ["customerName"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("John Doe") },
            ["datePlaced"] = new OpenApiSchema { Type = "string", Format = "date-time", Example = new OpenApiString(DateTime.UtcNow.ToString("o")) },
            ["items"] = new OpenApiSchema
            {
                Type = "array",
                Items = new OpenApiSchema
                {
                    Type = "object",
                    Properties = new Dictionary<string, OpenApiSchema>
                    {
                        ["itemId"] = new OpenApiSchema { Type = "integer", Example = new OpenApiInteger(1) },
                        ["name"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("Keyboard Logitech") },
                        ["quantity"] = new OpenApiSchema { Type = "integer", Example = new OpenApiInteger(2) },
                        ["location"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("Warehouse B") },
                        ["orderId"] = new OpenApiSchema { Type = "integer", Example = new OpenApiInteger(1) }
                    }
                }
            }
        }
    });

    // Configure Swagger to support JWT authentication via the "Authorization" header
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Require Bearer token in Swagger UI requests for secured endpoints
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
});

// Add in-memory caching support to optimize performance for controllers
builder.Services.AddMemoryCache();

var app = builder.Build();

// Create predefined roles and an admin user before the application starts
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    string[] roles = new[] { "Manager", "Admin", "User" };

    // Check and create each role if it doesn't exist
    foreach (var role in roles)
    {
        var roleExists = roleManager.RoleExistsAsync(role).Result;
        if (!roleExists)
        {
            roleManager.CreateAsync(new IdentityRole(role)).Wait();
        }
    }

    // Create a default admin user for development/testing purposes
    var adminEmail = "admin@example.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = "admin",
            Email = adminEmail,
        };
        await userManager.CreateAsync(adminUser, "Admin123!"); // Set a strong initial password
    }

    // Assign Admin role to the admin user if not already assigned
    if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
    {
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}

// Register a global exception handling middleware to catch and process unhandled exceptions uniformly
app.UseMiddleware<GlobalExceptionMiddleware>();

// Enable Swagger UI only in the development environment for API exploration and testing
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable authentication and authorization middlewares
app.UseAuthentication();
app.UseAuthorization();

// Map controller endpoints
app.MapControllers();

// Run the application
app.Run();
