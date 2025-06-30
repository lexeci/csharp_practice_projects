using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(); // Enables controller support

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var app = builder.Build();

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        Log.Information($"Global exception caught: {ex.Message}");
        context.Response.StatusCode = 500;
        await Log.CloseAndFlushAsync();
    }
});

// Enable routing and map controller endpoints
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // Maps attribute-defined routes in controllers
});

app.Run();
