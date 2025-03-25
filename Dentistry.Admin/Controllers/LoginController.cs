using Dentistry.Common;
using Dentistry.ViewModels.System.Users;
using Dentisty.Common;
using Dentisty.Data.Repositories;
using Dentisty.Data.Services.System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Dentistry.Admin.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;
        private readonly LoggerRepository _logs;

        public LoginController(UserService userService,
            LoggerRepository logs,
            IConfiguration configuration)
        {
            _logs = logs;
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
                return View(request);

            request.IpAddress = await Utilities.GetIpAddress();
            var result = await _userService.Authencate(request);
            if (result.ResultObj == null)
            {
                ModelState.AddModelError("", result.Message);
                return View();
            }
            var userPrincipal = this.ValidateToken(result.ResultObj);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.Now.AddDays(10),
                IsPersistent = request.RememberMe
            };
            var userId = userPrincipal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            // Lưu token vào cookie hoặc trả về trong response
            HttpContext.Response.Cookies.Append(SystemConstants.AppSettings.Token, result.ResultObj, new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(1),  // Cookie sẽ hết hạn sau 1 ngày
                IsEssential = true,  // Cookie là bắt buộc
                HttpOnly = true,  // Cookie không thể được truy cập từ JavaScript
                Secure = true,  // Cookie chỉ được gửi qua HTTPS
                SameSite = SameSiteMode.None
            });
            HttpContext.Response.Cookies.Append(SystemConstants.AppSettings.DefaultLanguageId, _configuration[SystemConstants.AppSettings.DefaultLanguageId]);
            await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        authProperties);

            await _userService.UpdateIpTimeZone(new Guid(userId), request.IpAddress, request.TimeZone);
            _logs.QueueLog("login done");
            return RedirectToAction("Index", "Home");
        }

        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = _configuration[SystemConstants.JwtTokens.Audience];
            validationParameters.ValidIssuer = _configuration[SystemConstants.JwtTokens.Issuer];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[SystemConstants.JwtTokens.Key]));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

            return principal;
        }
    }
}