using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var watch = Stopwatch.StartNew();
        _logger.LogInformation($"Incoming request: {context.Request.Method} {context.Request.Path}");

        await _next(context);

        watch.Stop();
        _logger.LogInformation($"Request completed with status code: {context.Response.StatusCode}, in {watch.ElapsedMilliseconds}ms");
    }
}