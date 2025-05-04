using Hotelia.Shared.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
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
            new(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Aud, jwtConfig.Value.Audience)
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