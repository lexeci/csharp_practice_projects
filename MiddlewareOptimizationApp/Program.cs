using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenLocalhost(5294);
        });

        var app = builder.Build();



        // Middleware для логування помилок
        app.Use(async (context, next) =>
        {
            await next();

            if (context.Response.StatusCode >= 400)
            {
                Console.WriteLine($"Security Event: {context.Request.Path} - Status Code: {context.Response.StatusCode}");
            }
        });

        // Middleware для перевірки "secure" параметра
        app.Use(async (context, next) =>
        {
            if (context.Request.Query["secure"] != "true")
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Simulated HTTPS Required\n");
                return; // Зупиняємо подальше виконання
            }
            await next();
        });

        // Middleware для перевірки інпуту
        app.Use(async (context, next) =>
        {
            var input = context.Request.Query["input"];

            if (!IsValidInput(input))
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid Input\n");
                return;
            }

            await next();
        });

        // Middleware для перевірки авторизації
        app.Use(async (context, next) =>
        {
            if (context.Request.Path == "/unauthorized")
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized Access\n");
                return;
            }

            await next();
        });

        // Middleware для аутентифікації
        app.Use(async (context, next) =>
        {
            var isAuthenticated = context.Request.Query["authenticated"] == "true";

            if (!isAuthenticated)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Access Denied\n");
                return;
            }

            context.Response.Cookies.Append("SecureCookie", "SecureData", new CookieOptions
            {
                HttpOnly = true,
                Secure = true
            });

            await next();
        });

        // Middleware для асинхронного процесу
        app.Use(async (context, next) =>
        {
            await Task.Delay(100);
            await next(); // Тепер пропускаємо далі
        });

        // Останнє повідомлення від сервера
        app.Run(async (context) =>
        {
            await context.Response.WriteAsync("Final Response from Application\n");
        });

        await app.RunAsync(); // Запускаємо сервер
    }

    // Функція перевірки коректності введених даних
    static bool IsValidInput(string input)
    {
        return string.IsNullOrEmpty(input) || (input.All(char.IsLetterOrDigit) && !input.Contains("<script>"));
    }
}
