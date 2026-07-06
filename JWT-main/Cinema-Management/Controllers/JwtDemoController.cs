using System.Security.Claims;
using Cinema_Management.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Cinema_Management.Controllers;

// email: admin@demo.com
// password: 123456

[ApiController]
[Route("api/jwt-demo")]
public class JwtDemoController : ControllerBase
{
    [HttpGet("public")]
    public IActionResult Public()
    {
        return Ok(new
        {
            message = "Public API: khong can JWT."
        });
    }

    [HttpGet("user")]
    [JwtAuthorize]
    public IActionResult UserOnly()
    {
        return Ok(new
        {
            message = "Authorized: JWT hop le, duoc truy cap chuc nang user.",
            userId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            email = User.FindFirstValue(ClaimTypes.Email),
            role = User.FindFirstValue(ClaimTypes.Role)
        });
    }

    [HttpGet("admin")]
    [JwtAuthorize("Admin")]
    public IActionResult AdminOnly()
    {
        return Ok(new
        {
            message = "Authorized: role Admin duoc truy cap chuc nang admin.",
            email = User.FindFirstValue(ClaimTypes.Email),
            role = User.FindFirstValue(ClaimTypes.Role)
        });
    }
}
