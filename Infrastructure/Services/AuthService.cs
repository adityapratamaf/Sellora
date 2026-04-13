using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Common.Models;
using Application.Services.Auth;
using Shared.DTO.Auth;

namespace Infrastructure.Services.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtSettings _jwt;

    public AuthService(UserManager<ApplicationUser> userManager, IOptions<JwtSettings> jwt)
    {
        _userManager = userManager;
        _jwt = jwt.Value;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            throw new Exception("Email & Password Required");
        }

        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            throw new Exception("Email Not Registered");
        }

        var valid = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!valid)
        {
            throw new Exception("Wrong Password");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "User";

        var token = GenerateToken(user, role);

        return new LoginResponse
        {
            Token = token,
            Email = user.Email!,
            Role = role,
            Name = user.Name
        };
    }

    public Task LogoutAsync()
    {
        return Task.CompletedTask;
    }

    private string GenerateToken(ApplicationUser user, string role)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            // new Claim(ClaimTypes.Name, user.Name ?? ""),
            new Claim(ClaimTypes.Name, user.Name ?? string.Empty),
            // new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(ClaimTypes.Role, role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}