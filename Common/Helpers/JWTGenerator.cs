using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MobileMend.Domain.Entities;
using Microsoft.Extensions.Configuration;


namespace MobileMend.Common.Helpers
{
    public class JWTGenerator
    {
        private readonly IConfiguration config;
        public JWTGenerator(IConfiguration _config)
        {
            config = _config;
        }

        public async Task<string> GenerateToken(User userdata) {

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userdata.UserID.ToString()),
                new Claim(ClaimTypes.Name, userdata.Name),
                new Claim(ClaimTypes.Role, userdata.Role)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
