using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
namespace Sandbox_Calc.Data
{
    public class Services
    {
        private readonly string _key;

        public Services(IConfiguration configuration)
        {
            _key = configuration["Jwt:Key"]!;
        }
        public (string token, int expiresInMinutes) GenerateToken(string username)
        {
            var keyBytes = Encoding.UTF8.GetBytes(_key);
            var tokenHandler = new JwtSecurityTokenHandler();

            var expires = DateTime.UtcNow.AddMinutes(60); // atau AddHours(1)
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }),
                Expires = expires,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(keyBytes),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return (jwt, 60); // dalam menit
        }
        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
