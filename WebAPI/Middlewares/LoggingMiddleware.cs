namespace WebAPI.Middlewares;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var requestId = Guid.NewGuid().ToString();
        context.Items["RequestId"] = requestId;
        
        var startTime = DateTime.UtcNow;

        try
        {
            _logger.LogInformation("HTTP Request Started: {Id} | {Method} {Path}", 
                requestId, context.Request.Method, context.Request.Path);

            await _next(context);

            var duration = (DateTime.UtcNow - startTime).TotalMilliseconds;
            _logger.LogInformation("HTTP Request Finished: {Id} | Status: {Status}",
                requestId, "successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "HTTP Request Failed: {Id} | Error: {Message}", requestId, ex.Message);
            throw;
        }
    }
}