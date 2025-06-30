using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add controllers support
builder.Services.AddControllers();

// Configure in-memory database for Identity
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("TestDB"));

// Configure Identity services
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Add authorization policies (without requiring authentication)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireITDepartment", policy =>
        policy.RequireClaim("Department", "IT"));
});

var app = builder.Build();

app.UseAuthorization();

// Enable controllers
app.MapControllers();

app.Run();