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
            // Lấy TimeZone từ Session (nếu có)
            var timeZoneId = context.Session.GetString("UserTimeZone");

            // Nếu chưa có, đặt mặc định là UTC
            if (string.IsNullOrEmpty(timeZoneId))
            {
                timeZoneId = "UTC";
            }

            // Lưu vào HttpContext.Items để sử dụng trong suốt request
            context.Items["UserTimeZone"] = timeZoneId;

            await _next(context);
        }
    }
}
