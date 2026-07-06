using System.ComponentModel.DataAnnotations;

namespace Cinema_Management.Models;

public class LoginRequest
{
    [Required(ErrorMessage = "Email khong duoc de trong")]
    [EmailAddress(ErrorMessage = "Email khong dung dinh dang")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mat khau khong duoc de trong")]
    public string Password { get; set; } = string.Empty;

    public string CaptchaToken { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}
