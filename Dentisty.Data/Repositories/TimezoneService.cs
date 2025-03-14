using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Repositories
{
    public class TimezoneService : ITimezoneService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TimezoneService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserTimeZoneId()
        {
            return _httpContextAccessor.HttpContext?.Items["UserTimezone"]?.ToString() ?? "UTC";
        }  
        public TimeZoneInfo GetUserTimeZone()
        {
            string timezoneId = _httpContextAccessor.HttpContext?.Items["UserTimezone"]?.ToString() ?? "UTC";
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
            return timeZone;
        }

    }

}
