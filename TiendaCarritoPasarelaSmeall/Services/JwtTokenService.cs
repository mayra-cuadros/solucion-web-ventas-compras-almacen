using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TiendaCarritoPasarelaSmeall.Models;

namespace TiendaCarritoPasarelaSmeall.Services
{
    public class JwtTokenService
    {
        private readonly SecurityOptions _opts;
        public JwtTokenService(IOptions<SecurityOptions> opts) => _opts = opts.Value;

        public (string token, DateTimeOffset expiresAt) CreateAccessToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opts.JwtSigningKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTimeOffset.UtcNow.AddMinutes(_opts.AccessTokenMinutes);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim("pwdChangedAt", user.PasswordChangedAtUtc?.ToUnixTimeSeconds().ToString() ?? "0")
            };

            var jwt = new JwtSecurityToken(
                issuer: _opts.JwtIssuer,
                audience: _opts.JwtAudience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expires.UtcDateTime,
                signingCredentials: creds
            );

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return (token, expires);
        }
    }
}
