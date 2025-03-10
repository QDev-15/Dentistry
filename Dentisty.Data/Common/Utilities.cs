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
            text = text ?? "";
            text = text.Trim();
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

        public static List<string> ExtractImageLinks(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return new List<string>();
            }
            HashSet<string> links = new HashSet<string>();
            string[] validExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".svg", ".bmp", ".tiff", ".ico" };

            bool IsValidImageLink(string url)
            {
                return validExtensions.Any(ext => url.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
            }

            // Regex tìm ảnh trong thẻ <img src="...">
            Regex imgSrcRegex = new Regex("<img[^>]+src=\\\"(.*?)\\\"", RegexOptions.IgnoreCase);
            foreach (Match match in imgSrcRegex.Matches(html))
            {
                string link = match.Groups[1].Value;
                if (IsValidImageLink(link)) links.Add(link);
            }

            // Regex tìm ảnh trong thuộc tính CSS background hoặc background-image
            Regex backgroundImgRegex = new Regex("background(?:-image)?\\s*:\\s*url\\((?:'|\\\")?(.*?)(?:'|\\\")?\\)", RegexOptions.IgnoreCase);
            foreach (Match match in backgroundImgRegex.Matches(html))
            {
                string link = match.Groups[1].Value;
                if (IsValidImageLink(link)) links.Add(link);
            }

            // Regex tìm ảnh trong thẻ <source srcset="...">
            Regex srcsetRegex = new Regex("<source[^>]+srcset=\\\"(.*?)\\\"", RegexOptions.IgnoreCase);
            foreach (Match match in srcsetRegex.Matches(html))
            {
                foreach (string link in match.Groups[1].Value.Split(','))
                {
                    string cleanLink = link.Trim().Split(' ')[0]; // Chỉ lấy URL, bỏ kích thước
                    if (IsValidImageLink(cleanLink)) links.Add(cleanLink);
                }
            }

            // Regex tìm ảnh trong thẻ <meta content="..."></meta>
            Regex metaContentRegex = new Regex("<meta[^>]+content=\\\"(.*?\\.(?:jpg|jpeg|png|gif|webp|svg|bmp|tiff|ico))\\\"", RegexOptions.IgnoreCase);
            foreach (Match match in metaContentRegex.Matches(html))
            {
                links.Add(match.Groups[1].Value);
            }

            // Regex tìm ảnh trong thẻ <link rel="preload" as="image" href="...">
            Regex preloadLinkRegex = new Regex("<link[^>]+rel=\\\"preload\\\"[^>]+as=\\\"image\\\"[^>]+href=\\\"(.*?)\\\"", RegexOptions.IgnoreCase);
            foreach (Match match in preloadLinkRegex.Matches(html))
            {
                string link = match.Groups[1].Value;
                if (IsValidImageLink(link)) links.Add(link);
            }

            // Regex tìm ảnh trong thẻ <video poster="...">
            Regex videoPosterRegex = new Regex("<video[^>]+poster=\\\"(.*?)\\\"", RegexOptions.IgnoreCase);
            foreach (Match match in videoPosterRegex.Matches(html))
            {
                string link = match.Groups[1].Value;
                if (IsValidImageLink(link)) links.Add(link);
            }

            // Regex tìm ảnh trong thẻ <object data="...">
            Regex objectDataRegex = new Regex("<object[^>]+data=\\\"(.*?)\\\"", RegexOptions.IgnoreCase);
            foreach (Match match in objectDataRegex.Matches(html))
            {
                string link = match.Groups[1].Value;
                if (IsValidImageLink(link)) links.Add(link);
            }

            // Regex tìm ảnh trong thẻ <embed src="...">
            Regex embedSrcRegex = new Regex("<embed[^>]+src=\\\"(.*?)\\\"", RegexOptions.IgnoreCase);
            foreach (Match match in embedSrcRegex.Matches(html))
            {
                string link = match.Groups[1].Value;
                if (IsValidImageLink(link)) links.Add(link);
            }

            // Regex tìm ảnh trong thuộc tính list-style-image
            Regex listStyleImgRegex = new Regex("list-style-image\\s*:\\s*url\\((?:'|\\\")?(.*?)(?:'|\\\")?\\)", RegexOptions.IgnoreCase);
            foreach (Match match in listStyleImgRegex.Matches(html))
            {
                string link = match.Groups[1].Value;
                if (IsValidImageLink(link)) links.Add(link);
            }

            // Regex tìm ảnh trong thuộc tính content: url(...)
            Regex contentUrlRegex = new Regex("content\\s*:\\s*url\\((?:'|\\\")?(.*?)(?:'|\\\")?\\)", RegexOptions.IgnoreCase);
            foreach (Match match in contentUrlRegex.Matches(html))
            {
                string link = match.Groups[1].Value;
                if (IsValidImageLink(link)) links.Add(link);
            }

            return new List<string>(links);
        }
    }
}
