using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Cinema_Management.Models;

namespace Cinema_Management.Services;

public sealed class JwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public JwtTokenResult GenerateToken(User user)
    {
        var now = DateTimeOffset.UtcNow;
        var expiresAt = now.AddMinutes(GetExpirationMinutes());

        var header = new Dictionary<string, object>
        {
            ["alg"] = "HS256",
            ["typ"] = "JWT"
        };

        var payload = new Dictionary<string, object>
        {
            ["iss"] = GetIssuer(),
            ["aud"] = GetAudience(),
            ["sub"] = user.UserID.ToString(),
            ["email"] = user.Email,
            ["name"] = user.FullName,
            ["role"] = user.Role,
            ["iat"] = now.ToUnixTimeSeconds(),
            ["exp"] = expiresAt.ToUnixTimeSeconds(),
            ["jti"] = Guid.NewGuid().ToString("N")
        };

        var encodedHeader = Base64UrlEncode(JsonSerializer.SerializeToUtf8Bytes(header));
        var encodedPayload = Base64UrlEncode(JsonSerializer.SerializeToUtf8Bytes(payload));
        var unsignedToken = $"{encodedHeader}.{encodedPayload}";
        var signature = Base64UrlEncode(Sign(unsignedToken));

        return new JwtTokenResult($"{unsignedToken}.{signature}", expiresAt);
    }

    public bool TryValidateToken(string token, out ClaimsPrincipal principal)
    {
        principal = new ClaimsPrincipal(new ClaimsIdentity());

        var parts = token.Split('.');
        if (parts.Length != 3)
        {
            return false;
        }

        var unsignedToken = $"{parts[0]}.{parts[1]}";
        var expectedSignature = Sign(unsignedToken);

        byte[] actualSignature;
        try
        {
            actualSignature = Base64UrlDecode(parts[2]);
        }
        catch
        {
            return false;
        }

        if (!CryptographicOperations.FixedTimeEquals(expectedSignature, actualSignature))
        {
            return false;
        }

        JsonDocument payload;
        try
        {
            payload = JsonDocument.Parse(Base64UrlDecode(parts[1]));
        }
        catch
        {
            return false;
        }

        using (payload)
        {
            var root = payload.RootElement;
            if (!IsExpectedString(root, "iss", GetIssuer()) ||
                !IsExpectedString(root, "aud", GetAudience()))
            {
                return false;
            }

            if (!root.TryGetProperty("exp", out var expElement) ||
                !expElement.TryGetInt64(out var exp) ||
                DateTimeOffset.UtcNow.ToUnixTimeSeconds() >= exp)
            {
                return false;
            }

            var userId = GetString(root, "sub");
            var email = GetString(root, "email");
            var name = GetString(root, "name");
            var role = GetString(root, "role");

            if (string.IsNullOrWhiteSpace(userId) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(role))
            {
                return false;
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userId),
                new(ClaimTypes.Email, email),
                new(ClaimTypes.Role, role),
                new("role", role)
            };

            if (!string.IsNullOrWhiteSpace(name))
            {
                claims.Add(new Claim(ClaimTypes.Name, name));
            }

            principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Jwt"));
            return true;
        }
    }

    private byte[] Sign(string value)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(GetSecretKey()));
        return hmac.ComputeHash(Encoding.UTF8.GetBytes(value));
    }

    private string GetIssuer() => _configuration["Jwt:Issuer"] ?? "CinemaManagement";

    private string GetAudience() => _configuration["Jwt:Audience"] ?? "CinemaManagementClient";

    private string GetSecretKey()
    {
        var secret = _configuration["Jwt:SecretKey"];
        if (string.IsNullOrWhiteSpace(secret))
        {
            throw new InvalidOperationException("Missing Jwt:SecretKey configuration.");
        }

        return secret;
    }

    private int GetExpirationMinutes()
    {
        return int.TryParse(_configuration["Jwt:ExpirationMinutes"], out var minutes) && minutes > 0
            ? minutes
            : 60;
    }

    private static bool IsExpectedString(JsonElement root, string name, string expected)
    {
        return string.Equals(GetString(root, name), expected, StringComparison.Ordinal);
    }

    private static string? GetString(JsonElement root, string name)
    {
        return root.TryGetProperty(name, out var element) ? element.GetString() : null;
    }

    private static string Base64UrlEncode(byte[] bytes)
    {
        return Convert.ToBase64String(bytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }

    private static byte[] Base64UrlDecode(string value)
    {
        var base64 = value.Replace('-', '+').Replace('_', '/');
        var padding = base64.Length % 4;
        if (padding > 0)
        {
            base64 = base64.PadRight(base64.Length + 4 - padding, '=');
        }

        return Convert.FromBase64String(base64);
    }
}

public sealed record JwtTokenResult(string Token, DateTimeOffset ExpiresAt);
