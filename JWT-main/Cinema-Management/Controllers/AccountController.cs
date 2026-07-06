namespace Cinema_Management.Controllers;

using Cinema_Management.Data;
using Cinema_Management.Models;
using Cinema_Management.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly JwtTokenService _jwtTokenService;

    public AccountController(ApplicationDbContext context, JwtTokenService jwtTokenService)
    {
        _context = context;
        _jwtTokenService = jwtTokenService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await FindUserOrDemoAsync(model.Email, model.Password);
        var isDemoUser = user != null && string.IsNullOrWhiteSpace(user.PasswordHash);

        if (user == null || (!isDemoUser && !IsPasswordValid(model.Password, user.PasswordHash)))
        {
            ModelState.AddModelError(string.Empty, "Sai email hoac mat khau");
            TempData["AlertError"] = "Sai email hoac mat khau. Vui long thu lai.";
            return View(model);
        }

        if (!user.Status)
        {
            ModelState.AddModelError(string.Empty, "Tai khoan da bi khoa");
            TempData["AlertError"] = "Tai khoan cua ban da bi khoa.";
            return View(model);
        }

        HttpContext.Session.SetString("UserEmail", user.Email);
        HttpContext.Session.SetString("UserRole", user.Role);
        HttpContext.Session.SetInt32("UserID", user.UserID);

        TempData["AlertSuccess"] = $"Dang nhap thanh cong! Xin chao {user.FullName} (Role: {user.Role})";

        return user.Role switch
        {
            "Admin" => RedirectToAction("Index", "Admin"),
            "Staff" => RedirectToAction("Index", "Staff"),
            _ => RedirectToAction("Index", "Home")
        };
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginApi([FromBody] ApiLoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await FindUserOrDemoAsync(request.Email, request.Password);
        var isDemoUser = user != null && string.IsNullOrWhiteSpace(user.PasswordHash);

        if (user == null || (!isDemoUser && !IsPasswordValid(request.Password, user.PasswordHash)))
        {
            return Unauthorized(new
            {
                message = "Sai email hoac mat khau.",
                demoAccounts = new[]
                {
                    new { email = "admin@demo.com", password = "123456", role = "Admin" },
                    new { email = "user@demo.com", password = "123456", role = "KhachHang" }
                }
            });
        }

        if (!user.Status)
        {
            return Unauthorized("Tai khoan da bi khoa");
        }

        var token = _jwtTokenService.GenerateToken(user);

        return Ok(new
        {
            message = isDemoUser ? "Dang nhap thanh cong bang tai khoan demo" : "Dang nhap thanh cong",
            demoMode = isDemoUser,
            token = token.Token,
            tokenType = "Bearer",
            expiresAt = token.ExpiresAt,
            user = new
            {
                user.UserID,
                user.FullName,
                user.Email,
                user.Role
            }
        });
    }

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        TempData["AlertSuccess"] = "Ban da dang xuat thanh cong.";
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(AuthViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var email = model.Email.Trim().ToLowerInvariant();
        var emailExists = await _context.Users.AnyAsync(u => u.Email.ToLower() == email);

        if (emailExists)
        {
            AddDuplicateAccountError();
            return View(model);
        }

        var user = new User
        {
            FullName = model.FullName.Trim(),
            Email = email,
            PhoneNumber = model.PhoneNumber.Trim(),
            DOB = model.DateOfBirth,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
            Status = true,
            Role = "KhachHang"
        };

        _context.Users.Add(user);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException exception) when (IsDuplicateAccountError(exception))
        {
            AddDuplicateAccountError();
            return View(model);
        }

        TempData["SuccessMessage"] = "Dang ky thanh cong. Hay dang nhap.";
        return RedirectToAction(nameof(Login));
    }

    private async Task<User?> FindUserOrDemoAsync(string email, string password)
    {
        try
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }
        catch (SqlException)
        {
            return FindDemoUser(email, password);
        }
    }

    private static User? FindDemoUser(string email, string password)
    {
        if (password != "123456")
        {
            return null;
        }

        return email.Trim().ToLowerInvariant() switch
        {
            "admin@demo.com" or "admin@movieticket.com" => new User
            {
                UserID = 1,
                FullName = "Admin Demo",
                Email = "admin@demo.com",
                Role = "Admin",
                Status = true
            },
            "user@demo.com" or "hoangpm@gmail.com" => new User
            {
                UserID = 2,
                FullName = "User Demo",
                Email = "user@demo.com",
                Role = "KhachHang",
                Status = true
            },
            "staff@demo.com" => new User
            {
                UserID = 3,
                FullName = "Staff Demo",
                Email = "staff@demo.com",
                Role = "Staff",
                Status = true
            },
            _ => null
        };
    }

    private void AddDuplicateAccountError()
    {
        ModelState.AddModelError(nameof(AuthViewModel.Email), "Tai khoan da co");
    }

    private static bool IsDuplicateAccountError(DbUpdateException exception)
    {
        return exception.InnerException is SqlException sqlException
               && sqlException.Errors
                   .Cast<SqlError>()
                   .Any(error => error.Number is 2601 or 2627);
    }

    private static bool IsPasswordValid(string password, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            return false;
        }

        if (passwordHash.StartsWith("$2", StringComparison.Ordinal))
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, passwordHash);
            }
            catch
            {
                return false;
            }
        }

        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var hashBytes = SHA256.HashData(passwordBytes);
        var sha256Hash = Convert.ToHexString(hashBytes);
        return string.Equals(sha256Hash, passwordHash, StringComparison.OrdinalIgnoreCase);
    }
}
