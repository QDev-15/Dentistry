using Dentistry.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data
{
    public class TimeZoneMiddleware
    {
        private readonly RequestDelegate _next;

        public TimeZoneMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Cookies.TryGetValue("timezone", out var timezone))
            {
                context.Items["UserTimezone"] = timezone;
                SystemConstants.TimeZoneDefaultId = timezone;
            }
            else
            {
                context.Items["UserTimezone"] = "Asia/Ho_Chi_Minh"; // Mặc định nếu không có
                SystemConstants.TimeZoneDefaultId = "Asia/Ho_Chi_Minh";
            }

            await _next(context);
        }
    }
}
