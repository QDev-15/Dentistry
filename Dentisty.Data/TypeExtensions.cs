﻿using Dentistry.Common;
using Dentisty.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data
{
    public static class TypeExtensions 
    {
        public static DateTime ConvertUtcToLocal(this DateTime value, IHttpContextAccessor httpContextAccessor)
        {
            var context = httpContextAccessor.HttpContext;

            if (context == null) return value; // Nếu không có HttpContext, trả về UTC

            var timeZoneId = context.Items["UserTimeZone"]?.ToString() ?? "UTC";
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

            return TimeZoneInfo.ConvertTimeFromUtc(value, timeZone);
        }
        public static DateTime UTC(this DateTime value)
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(SystemConstants.TimeZoneUTC);
            return TimeZoneInfo.ConvertTimeToUtc(value, timeZone);
        }
        public static DateTime Local(this DateTime value)
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(SystemConstants.TimeZoneDefaultId);
            return TimeZoneInfo.ConvertTimeFromUtc(value, timeZone);
        }
        public static DateTime Local(this DateTime value, string timezoneId) {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(value, timeZone);
        }    
        public static DateTime Local(this DateTime value, TimeZoneInfo timezone) {
            return TimeZoneInfo.ConvertTimeFromUtc(value, timezone);
        }
        /// <summary>
        /// Remove img if not start ['assets', 'uploads', 'data:image', 'http']
        /// </summary>
        /// <param name="safeDescription"></param>
        /// <returns></returns>
        public static string ValidateImg(this string safeDescription)
        {
            safeDescription = System.Text.RegularExpressions.Regex.Replace(
                safeDescription,
                "<img[^>]+src=[\"'](?!https?://|/assets/|/uploads/|data:image/)[^\"']+[\"'][^>]*>",
                "",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            return safeDescription;
        }
        /// <summary>
        /// Example: abc-def-123
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToSlus(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return Utilities.ConvertToSlug(value);
        }
        /// <summary>
        /// Example: abc_def_123
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToSlus_V2(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return Utilities.ConvertToSlug(value).Replace('-', '_');
        }
        /// <summary>
        ///  dd/MM/yyyy - 15/09/2009
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Format_DMY(this DateTime value)
        {
            return value.ToString("d");
        }
        public static string GetTimestamp(this DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }
    }
}
