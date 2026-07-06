using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cinema_Management.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class JwtAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly string[] _roles;

    public JwtAuthorizeAttribute(params string[] roles)
    {
        _roles = roles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        if (user.Identity?.IsAuthenticated != true)
        {
            context.Result = new UnauthorizedObjectResult(new
            {
                message = "Unauthorized: hay gui JWT trong header Authorization: Bearer <token>."
            });
            return;
        }

        if (_roles.Length > 0 && !_roles.Any(user.IsInRole))
        {
            context.Result = new ObjectResult(new
            {
                message = "Forbidden: token hop le nhung role khong du quyen.",
                requiredRoles = _roles,
                currentRole = user.FindFirstValue(ClaimTypes.Role)
            })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }
    }
}
