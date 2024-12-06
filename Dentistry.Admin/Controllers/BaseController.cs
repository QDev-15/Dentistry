using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Dentistry.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dentistry.Admin.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var sessions = context.HttpContext.Request.Cookies[Constants.AppSettings.Token];
            //var sessions = context.HttpContext.Session.GetString(Constants.AppSettings.Token);
            if (sessions == null)
            {
                context.Result = new RedirectToActionResult("Index", "Login", null);
            }
            base.OnActionExecuting(context);
        }
    }
}