using DatingApp.Backend.Configs.Settings;
using DatingApp.Backend.Core.Entities;
using DatingApp.Backend.Dtos.User;
using DatingApp.Backend.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DatingApp.Backend.Services
{
    public class TokenService : ITokenService
    {
        private readonly TokenSettings _tokenSettings;
        private readonly SymmetricSecurityKey _key;
        public TokenService(IOptions<TokenSettings> tokenSettings)
        {
            _tokenSettings = tokenSettings.Value;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(s: _tokenSettings.Secret));
        }
        public string CreateToken(UserCredentialsDto appUser)
        {
            var claims = new List<Claim>
            { 
                new Claim(JwtRegisteredClaimNames.NameId, appUser.UserName)
            };
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);  
        }
    }
}
