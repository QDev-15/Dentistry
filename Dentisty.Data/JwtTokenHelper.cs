using Dentistry.Common;
using Dentistry.Data.GeneratorDB.Entities;
using Dentisty.Data.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Dentisty.Data
{
    public class JwtTokenHelper
    {
        private readonly AppConfigService _appConfigService;

        public JwtTokenHelper(AppConfigService appConfigService)
        {
            _appConfigService = appConfigService;
        }

        public string GenerateToken(AppUser user, List<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.GivenName,user.FirstName),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim("DisplayName", user.DisplayName),
                new Claim("FullName", $"{user.FirstName} {user.LastName}"),
            };
            // Mỗi role phải là 1 claim riêng để [Authorize(Roles = "...")] / User.IsInRole(...) nhận diện đúng
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appConfigService.JwtTokens.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),  // Thêm claim vào Subject
                Expires = DateTime.Now.AddMinutes(_appConfigService.JwtTokens.ExpiresInMinutes),  // Thời gian hết hạn
                SigningCredentials = creds,
                Issuer = _appConfigService.JwtTokens.Issuer,  // Người phát hành
                Audience = _appConfigService.JwtTokens.Audience  // Đối tượng nhận token
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        
    }
}
