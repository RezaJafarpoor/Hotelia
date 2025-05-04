using Microsoft.IdentityModel.Tokens;

namespace Hotelia.Shared.Auth;

public record JwtConfig(string Issuer, string Audience, string Secret,int ExpiryInMinute);