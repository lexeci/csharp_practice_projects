public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;  // Delegate pointing to the next middleware in the pipeline
    private readonly ILogger<GlobalExceptionMiddleware> _logger;  // Logger to record error details

    // Constructor to inject dependencies
    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    // Middleware execution method that intercepts the HTTP context
    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            // Call the next middleware component in the pipeline
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            // Log the exception with error severity
            _logger.LogError(ex, "Unhandled exception caught in middleware.");

            // Set HTTP response status code to 500 Internal Server Error
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            // Specify that the response content is JSON
            httpContext.Response.ContentType = "application/json";

            // Prepare a generic error message response to return to the client
            var response = new
            {
                Message = "An unexpected error occurred. Please try again later."
            };

            // Write the JSON response asynchronously
            await httpContext.Response.WriteAsJsonAsync(response);
        }
    }
}
