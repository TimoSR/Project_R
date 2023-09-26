using Infrastructure.Utilities._Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Middleware;

/*
 * 
 */
public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ITokenHandler _tokenHandler;
    private readonly ILogger<JwtMiddleware> _logger;

    public JwtMiddleware(RequestDelegate next, ITokenHandler tokenHandler, ILogger<JwtMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _tokenHandler = tokenHandler ?? throw new ArgumentNullException(nameof(tokenHandler));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Skip middleware for endpoints marked with [AllowAnonymous]
        var endpoint = context.GetEndpoint();
        
        if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
        {
            await _next(context);
            return;
        }
        
        if (!context.Request.Headers.TryGetValue(AuthConstants.AuthorizationHeader, out var authHeader))
        {
            await ProceedToNextMiddleware(context).ConfigureAwait(false);
            return;
        }

        var header = authHeader.ToString();
        if (!header.StartsWith(AuthConstants.BearerPrefix, StringComparison.OrdinalIgnoreCase))
        {
            await RespondWithBadRequest(context, "Authorization header does not contain a Bearer token.").ConfigureAwait(false);
            return;
        }

        var token = header.Substring(AuthConstants.BearerPrefix.Length).Trim();
        if (string.IsNullOrEmpty(token))
        {
            await ProceedToNextMiddleware(context).ConfigureAwait(false);
            return;
        }

        try
        {
            using (_logger.BeginScope($"JWT Validation for Token: {token}"))
            {
                var principal = _tokenHandler.DecodeToken(token);
                context.User = principal; // Setting the user.
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Token validation failed");
            await RespondWithUnauthorized(context, "Unauthorized").ConfigureAwait(false);
            return;
        }

        await ProceedToNextMiddleware(context).ConfigureAwait(false);
    }

    private async Task ProceedToNextMiddleware(HttpContext context)
    {
        await _next(context).ConfigureAwait(false);
    }

    private async Task RespondWithBadRequest(HttpContext context, string message)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsync(message).ConfigureAwait(false);
    }

    private async Task RespondWithUnauthorized(HttpContext context, string message)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsync(message).ConfigureAwait(false);
    }
}