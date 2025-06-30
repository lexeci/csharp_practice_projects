using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var config = builder.Configuration;
        var secretKey = Encoding.UTF8.GetBytes(config["JwtSettings:SecretKey"]);

        builder.Services.AddControllers();

        // Web server configuration
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenLocalhost(5000);
        });

        // JWT Authentication configuration
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey)
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return context.Response.WriteAsync("{\"error\": \"Invalid or expired token\"}");
                    }
                };
            });

        builder.Services.AddAuthorization();

        var app = builder.Build();

        // Error handling middleware
        app.Use(async (context, next) =>
        {
            try
            {
                await next();
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                var errorResponse = new { error = "Internal Server Error", details = ex.Message };
                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        });

        // Logging middleware
        app.Use(async (context, next) =>
        {
            var method = context.Request.Method;
            var path = context.Request.Path;
            var timestamp = DateTime.UtcNow;

            Console.WriteLine($"[{timestamp}] {method} {path}");

            await next();

            var statusCode = context.Response.StatusCode;
            Console.WriteLine($"[{timestamp}] {method} {path} - Status Code: {statusCode}");
        });

        // Route definitions
        app.MapGet("/", () => "You are welcome!");
        app.MapControllers();

        await Task.Run(() => app.RunAsync());
    }
}
