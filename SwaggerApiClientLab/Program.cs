using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using CustomNamespace; // Замініть на ваш простір імен

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Додаємо підтримку контролерів і Swagger
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Вмикаємо Swagger
        app.UseRouting();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        });

        // Додаємо маршрутизацію
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        // Тестовий маршрут
        app.MapGet("/", () => "Hello, World!");

        // Чекаємо завершення роботи сервера
        await Task.Run(() => app.RunAsync());
        
        // Після запуску сервера виконуємо запити
        var httpClient = new HttpClient();
        var client = new CustomApiClient("http://localhost:5000", httpClient);
        var user = await client.GetUserAsync(1);
        Console.WriteLine($"Fetched User: {user}");
    }
}