var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpLogging((o) => { });

var app = builder.Build();

app.UseHttpLogging();

app.Use(async (context, next) =>
{
    Console.WriteLine($"Logic run before {context.Request.Path}");
    await next.Invoke();
    Console.WriteLine($"Logic run after {context.Request.Path}");
});

app.MapGet("/", () => "Hello World!");
app.MapGet("/api", () => "Take your actions from the API");
app.MapGet("/api/products/{id:int:min(0)}", (int id) => $"Product {id}");

app.Run();
