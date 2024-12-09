using Dentistry.Common.Constants;
using Dentistry.Data.GeneratorDB.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Common
{
    public class JwtTokenHelper
    {
        private readonly IConfiguration _config;

        public JwtTokenHelper(IConfiguration configuration)
        {
            _config = configuration;
        }

        public string GenerateToken(AppUser user, List<string> roles)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.GivenName,user.FirstName),
                new Claim(ClaimTypes.Role, string.Join(";",roles)),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim("DisplayName", user.DisplayName),
                new Claim("FullName", $"{user.FirstName} {user.LastName}"),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config[Constants.JwtTokens.Key]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),  // Thêm claim vào Subject
                Expires = DateTime.UtcNow.AddHours(1),  // Thời gian hết hạn
                SigningCredentials = creds,
                Issuer = _config[Constants.JwtTokens.Issuer],  // Người phát hành
                Audience = _config[Constants.JwtTokens.Audience]  // Đối tượng nhận token
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
