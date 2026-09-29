using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Inventory.Api.Entities;
using Inventory.Api.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Inventory.Api.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(User user)
        {
            var key = _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException(
                    "JWT key is not configured."
                );

            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var expiryMinutes =
                _configuration.GetValue<int>("Jwt:ExpiryMinutes");

            var claims = new List<Claim>
            {
                new(
                    JwtRegisteredClaimNames.Sub,
                    user.Id.ToString()
                ),

                new(
                    JwtRegisteredClaimNames.Email,
                    user.Email
                ),

                new(
                    ClaimTypes.Name,
                    user.FullName
                ),

                new(
                    ClaimTypes.Role,
                    user.Role
                ),

                new(
                    JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString()
                )
            };

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key)
            );

            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}
