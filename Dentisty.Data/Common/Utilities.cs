using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Catalog.Categories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Dentisty.Data.Common
{
    public static class Utilities
    {
        private static readonly char[] Characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
        private static readonly Random Random = new Random();

        public static string GenerateRandomString(int length)
        {
            var chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = Characters[Random.Next(Characters.Length)];
            }
            return new string(chars);
        }
        public static string HashPassword(string password)
        {
            var hasher = new PasswordHasher<object>(); // Use object because it needs a class type, but it won't be used.
            var hashedPassword = hasher.HashPassword(null, password); // null is for the user object, since we are not using it here
            return hashedPassword;
        }
        public static async Task<bool> VerifyPassword(UserManager<AppUser> userManager, AppUser user, string enteredPassword)
        {
            var result = await userManager.CheckPasswordAsync(user, enteredPassword);
            return result; // Returns true if the password is correct, false otherwise
        }

        public static string ConvertToSlug(string text)
        {
            // Chuyển thành chữ thường
            text = text.ToLowerInvariant();

            // Loại bỏ dấu
            text = RemoveDiacritics(text);

            // Thay thế khoảng trắng bằng dấu gạch ngang
            text = Regex.Replace(text, @"\s+", "-");

            return text;
        }

        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var character in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(character);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(character);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }


        public static async Task<string> RenderViewToStringAsync(
            this Controller controller, // `this` chỉ định đây là extension method
            ICompositeViewEngine viewEngine,
            string viewName,
            object model)
        {
            controller.ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                var viewResult = viewEngine.FindView(controller.ControllerContext, viewName, false);
                if (!viewResult.Success)
                {
                    throw new InvalidOperationException($"View {viewName} not found");
                }

                var viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);
                return writer.ToString();
            }
        }
        /// <summary>
        /// Get Ip Location on Internet
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetInternetIpAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                return await client.GetStringAsync("https://api64.ipify.org");
            }
        }
        /// <summary>
        /// Get Ip Location on Local
        /// </summary>
        /// <returns></returns>
        public static string GetInternerIp()
        {
            string hostName = Dns.GetHostName(); // Lấy tên máy
            string localIp = Dns.GetHostAddresses(hostName)
                                .FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?
                                .ToString();
            return localIp ?? "Không thể xác định IP";
        }
        public static async Task<string> GetIpAddress()
        {
            try
            {
                // Thử lấy IP public
                using (HttpClient client = new HttpClient())
                {

                    //var ip = await client.GetStringAsync("https://api64.ipify.org"); // ipv6
                    var ip = await client.GetStringAsync("https://api.ipify.org"); // ipv4
                    return ip;
                }
            }
            catch
            {
                // Nếu thất bại (không có internet), lấy IP nội bộ
                return GetInternerIp();
            }
        }

    }
}
