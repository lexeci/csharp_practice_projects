var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var blogs = new List<Blog>()
{
    new Blog { Title = "My first blog", Body = "This is my first blog." },
    new Blog { Title = "My second blog", Body = "This is my second blog." },
};

app.Use(async (context, next) =>
{
    var startTime = DateTime.UtcNow;
    await next.Invoke();
    var duration = DateTime.UtcNow.Subtract(startTime);
    Console.WriteLine($"Duration of the operation: {duration.TotalSeconds}s");
});

app.UseWhen(
    context => context.Request.Method != "GET",
    appBuilder => appBuilder.Use(async (context, next) =>
    {
        var extractedPassword = context.Request.Headers["X-Api-Key"];
        if (extractedPassword != "badExample")
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Bad Request. Invalid Api Key");
            return; // Зупиняємо подальше виконання Middleware
        }

        await next.Invoke(); // Викликаємо наступний Middleware тільки якщо API Key валідний
    })
);

app.MapGet("/", () => "I'm the root!");

app.MapGet("api/blogs", () => Results.Ok(blogs));

app.MapGet("api/blogs/{id}", (int id) =>
{
    if (id < 0 || id > blogs.Count)
    {
        return Results.NotFound();
    }
    
    return Results.Ok(blogs[id]);
});

app.MapPost("api/blogs", (Blog blog) =>
{
    blogs.Add(blog);
    return Results.Created($"api/blogs/{blogs.Count - 1}", blog);
});


app.MapDelete("api/blogs/{id}", (int id) =>
{
    if (id < 0 || id > blogs.Count)
    {
        return Results.NotFound();
    }

    blogs.RemoveAt(id);
    return Results.NoContent();
});

app.MapPut("api/blogs/{id}", (int id, Blog blog) =>
{
    if (id < 0 || id > blogs.Count)
    {
        return Results.NotFound();
    }
    blogs[id] = blog;
    return Results.Ok(blog);
});

app.Run();

public class Blog
{
    public string Title { get; set; }
    public string Body { get; set; }
}