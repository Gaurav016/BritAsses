using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly InMemoryRefreshTokenStore _refreshTokenStore;

    public AuthController(InMemoryRefreshTokenStore refreshTokenStore)
    {
        _refreshTokenStore = refreshTokenStore;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        // Validate user credentials (demo only)
        if (model.Username == "user" && model.Password == "password")
        {
            var token = GenerateJwtToken(model.Username);
            var refreshToken = GenerateRefreshToken();
            _refreshTokenStore.SaveRefreshToken(model.Username, refreshToken);
            return Ok(new { token, refreshToken });
        }
        return Unauthorized();
    }

    [HttpPost("refresh")]
    public IActionResult Refresh([FromBody] RefreshRequest request)
    {
        var username = "user"; // For demo, use a fixed username
        if (_refreshTokenStore.ValidateRefreshToken(username, request.RefreshToken))
        {
            var newToken = GenerateJwtToken(username);
            var newRefreshToken = GenerateRefreshToken();
            _refreshTokenStore.SaveRefreshToken(username, newRefreshToken);
            return Ok(new { token = newToken, refreshToken = newRefreshToken });
        }
        return Unauthorized();
    }

    private string GenerateJwtToken(string username)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username)
        };
        //long iat = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        //claims.Add(new Claim(JwtRegisteredClaimNames.Iat, iat.ToString()));
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authentication"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "yourIssuer",
            audience: "yourAudience",
            claims: claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString();
    }
}

public class LoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class RefreshRequest
{
    public string RefreshToken { get; set; }
}
