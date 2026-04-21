using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OnlineBazar.Application.Interfaces;
using OnlineBazar.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineBazar.Infrastructure.Security;

public class JwtTokenGenerator(IConfiguration config) : IJwtTokenGenerator
{
    public string Generate(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"] ?? "change-this-super-secret-key"));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.Name, user.FullName)
        };

        claims.AddRange(user.Roles.Select(r => new Claim(ClaimTypes.Role, r.Name)));

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"] ?? "OnlineBazar",
            audience: config["Jwt:Audience"] ?? "OnlineBazarClients",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
