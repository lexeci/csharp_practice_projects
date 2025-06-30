using DIProjectTest.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IMyService, MyService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    var myService = context.RequestServices.GetRequiredService<IMyService>();
    myService.LogCreation("First Middleware"); 
    await next();
});

app.Use(async (context, next) =>
{
    var myService = context.RequestServices.GetRequiredService<IMyService>();
    myService.LogCreation("Second Middleware"); 
    await next();
});

// Transient-сервіс можна отримувати без IServiceScope
app.MapGet("/", (IMyService myService) =>
{ 
    myService.LogCreation("Root");
});

app.Run();
