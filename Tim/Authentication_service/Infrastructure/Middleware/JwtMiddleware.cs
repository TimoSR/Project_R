using Infrastructure.Utilities._Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ITokenHandler _tokenHandler;
    private readonly ILogger<JwtMiddleware> _logger;

    public JwtMiddleware(RequestDelegate next, ITokenHandler tokenHandler, ILogger<JwtMiddleware> logger)
    {
        _next = next;
        _tokenHandler = tokenHandler;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        string token = context.Request.Headers["Authorization"].ToString().Split("Bearer ")[^1];

        if (string.IsNullOrEmpty(token))
        {
            await _next(context);
            return;
        }

        try
        {
            _tokenHandler.DecodeToken(token); // Assume DecodeToken throws an exception for invalid tokens
        }
        catch (Exception ex)
        {
            _logger.LogError($"Token validation failed: {ex.Message}");
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }

        await _next(context);
    }
}