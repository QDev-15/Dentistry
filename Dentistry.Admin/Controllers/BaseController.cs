using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dentistry.Admin.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        public bool IsTokenExpired(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(token))
            {
                throw new ArgumentException("Invalid JWT token");
            }

            var jwtToken = handler.ReadJwtToken(token);

            var expiration = jwtToken.ValidTo; // UTC time
            return expiration < DateTime.UtcNow;
        }
        protected bool IsAuthenticated()
        {
            var token = HttpContext.Session.GetString("Token");
            if (token != null && IsTokenExpired(token))
            {
                return false;
            }
            var cookie = Request.Cookies.ContainsKey("AuthToken");
            return cookie;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!IsAuthenticated())
            {
                context.Result = RedirectToAction("Index", "Login");
            }

            base.OnActionExecuting(context);
        }
    }
}