using Cinema_Management.Services;

namespace Cinema_Management.Middleware;

public sealed class JwtAuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public JwtAuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, JwtTokenService jwtTokenService)
    {
        var authorization = context.Request.Headers.Authorization.ToString();

        if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            var token = authorization["Bearer ".Length..].Trim();
            if (jwtTokenService.TryValidateToken(token, out var principal))
            {
                context.User = principal;
            }
        }

        await _next(context);
    }
}
