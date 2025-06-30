var builder = WebApplication.CreateBuilder(args);

// Додаємо логування HTTP-запитів
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});

// Додаємо аутентифікацію
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
    })
    .AddCookie("Cookies");

// Додаємо авторизацію
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseHttpLogging();
app.UseAuthentication();
app.UseAuthorization(); // Використовуємо авторизацію

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
else
{
    app.UseDeveloperExceptionPage();
}

// Middleware для логування запитів
app.Use(async (context, next) =>
{
    Console.WriteLine($"Request Path: {context.Request.Path}");
    await next.Invoke();
    Console.WriteLine($"Response Status Code: {context.Response.StatusCode}");
});

// Middleware для вимірювання часу виконання запитів
app.Use(async (context, next) =>
{
    var startTime = DateTime.UtcNow;
    Console.WriteLine($"Start Time: {startTime}");
    await next.Invoke();
    var duration = DateTime.UtcNow.Subtract(startTime);
    Console.WriteLine($"Duration: {duration.TotalMilliseconds}ms");
});

app.MapGet("/", () => "Hello World!");

app.Run();