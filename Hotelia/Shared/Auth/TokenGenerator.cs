using Hotelia.Shared.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hotelia.Shared.Auth;

public sealed class TokenGenerator(IOptions<JwtConfig> jwtConfig)
{
    public string GenerateToken(User user)
    {
        var secretKey = jwtConfig.Value.Secret;
        var claims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Aud, jwtConfig.Value.Audience),
            new(JwtRegisteredClaimNames.Iss, jwtConfig.Value.Issuer),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Value.Secret));
        var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken
        (
            issuer: jwtConfig.Value.Issuer,
            claims:claims,
            expires:DateTime.UtcNow.AddMinutes(jwtConfig.Value.ExpiryInMinute),
            signingCredentials:credential
            );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}