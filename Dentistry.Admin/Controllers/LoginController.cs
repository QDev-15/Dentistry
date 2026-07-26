using Dentistry.Common;
using Dentistry.ViewModels.System.Users;
using Dentisty.Common;
using Dentisty.Data.Repositories;
using Dentisty.Data.Services;
using Dentisty.Data.Services.System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserService _userService;
        private readonly LoggerRepository _logs;
        private readonly AppConfigService _appConfigService;

        public LoginController(UserService userService,
            LoggerRepository logs, AppConfigService appConfigService)
        {
            _appConfigService = appConfigService;
            _logs = logs;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            request.IpAddress = await Utilities.GetIpAddress();
            var result = await _userService.Authencate(request); // kiểm tra đăng nhập return resultObj là token jwt
            if (!result.IsSuccessed)
            {
                ModelState.AddModelError("", result.Message);
                return View();
            }
            // Lưu token vào cookie hoặc trả về trong response
            HttpContext.Response.Cookies.Append(SystemConstants.AppSettings.Token, result.ResultObj, new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(_appConfigService.JwtTokens.ExpiresInMinutes),  // Cookie sẽ hết hạn sau 10 ngày
                IsEssential = true,  // Cookie là bắt buộc
                HttpOnly = true,  // Cookie không thể được truy cập từ JavaScript
                Secure = true,  // Cookie chỉ được gửi qua HTTPS
                SameSite = SameSiteMode.Lax
            });
            HttpContext.Response.Cookies.Append(SystemConstants.AppSettings.DefaultLanguageId, _appConfigService.DefaultLanguageId.ToString());
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _userService.UpdateIpTimeZone(new Guid(userId), request.IpAddress, request.TimeZone);
            _logs.QueueLog("login done");
            return RedirectToAction("Index", "Home");
        }
    }
}