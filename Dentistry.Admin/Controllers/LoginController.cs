using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Dentistry.ViewModels.System.Users;
using Dentistry.Data.Interfaces;
using Dentistry.Common.Constants;

namespace Dentistry.Admin.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userService;

        public LoginController( IUserRepository userService,
            IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return View(ModelState);

            var result = await _userService.Authencate(request);
            if (result.ResultObj == null)
            {
                ModelState.AddModelError("", result.Message);
                return View();
            }
            var userPrincipal = this.ValidateToken(result.ResultObj);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(10),
                IsPersistent = request.RememberMe
            };
            // Lưu token vào cookie hoặc trả về trong response
            HttpContext.Response.Cookies.Append(Constants.AppSettings.Token, result.ResultObj, new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(1),  // Cookie sẽ hết hạn sau 1 ngày
                IsEssential = true,  // Cookie là bắt buộc
                HttpOnly = true,  // Cookie không thể được truy cập từ JavaScript
                Secure = true  // Cookie chỉ được gửi qua HTTPS
            });
            HttpContext.Response.Cookies.Append(Constants.AppSettings.DefaultLanguageId, _configuration[Constants.AppSettings.DefaultLanguageId]);
            //HttpContext.Session.SetString(Constants.AppSettings.DefaultLanguageId, _configuration[Constants.AppSettings.DefaultLanguageId]);
            //HttpContext.Session.SetString(Constants.AppSettings.Token, result.ResultObj);
            await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        authProperties);

            return RedirectToAction("Index", "Home");
        }

        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = _configuration[Constants.JwtTokens.Audience];
            validationParameters.ValidIssuer = _configuration[Constants.JwtTokens.Issuer];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[Constants.JwtTokens.Key]));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

            return principal;
        }
    }
}