using Ekinci.Common.JwtModels;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ekinci.Common.Helpers
{
    public class CustomTokenHandler
    {
        private readonly IConfiguration _configuration;

        public CustomTokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Token CreateAccessToken(List<Claim> claims)
        {
            Token token = new Token();
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));
            SigningCredentials signingCredentials = new SigningCredentials(key, "HS256");
            token.Expiration = DateTime.UtcNow.AddYears(1);
            string issuer = _configuration["Token:Issuer"];
            string audience = _configuration["Token:Audience"];
            DateTime? expires = token.Expiration;
            DateTime? notBefore = DateTime.UtcNow;
            SigningCredentials signingCredentials2 = signingCredentials;
            JwtSecurityToken token2 = new JwtSecurityToken(issuer, audience, claims, notBefore, expires, signingCredentials2);
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            token.AccessToken = jwtSecurityTokenHandler.WriteToken(token2);
            token.RefreshToken = Guid.NewGuid().ToString();
            return token;
        }
    }
}
