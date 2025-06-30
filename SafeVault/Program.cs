using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.AddScoped<AuthenticationService>();

// --- Services ---
builder.Services.AddRazorPages();
builder.Services.AddScoped<DatabaseService>();
builder.Services.AddSingleton<JwtService>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        var jwtService = new JwtService();
        options.TokenValidationParameters = jwtService.GetValidationParameters();

        options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.HttpContext.Request.Cookies["jwt"];
                if (!string.IsNullOrEmpty(token))
                {
                    context.Token = token;
                }
                return Task.CompletedTask;
            },
            OnForbidden = context =>
            {
                context.Response.Redirect("/AccessDenied");
                return Task.CompletedTask;
            },
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
    options.AddPolicy("Everyone", policy => policy.RequireRole("user"));
});

// --- App ---
var app = builder.Build();

// Exception Handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.Run();
