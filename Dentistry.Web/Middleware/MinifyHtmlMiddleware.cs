using System.Text;

namespace Dentistry.Web.Middleware
{
    public class MinifyHtmlMiddleware
    {
        private readonly RequestDelegate _next;

        public MinifyHtmlMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Chỉ minify HTML khi Content-Type là text/html
            if (context.Response.HasStarted ||
                context.Request.Path.Value.Contains(".") ||
                !context.Request.Headers["Accept"].ToString().Contains("text/html"))
            {
                await _next(context);
                return;
            }

            var originalBodyStream = context.Response.Body;
            using (var memoryStream = new MemoryStream())
            {
                context.Response.Body = memoryStream;

                await _next(context);

                context.Response.Body = originalBodyStream;
                memoryStream.Seek(0, SeekOrigin.Begin);

                using (var reader = new StreamReader(memoryStream))
                {
                    var html = await reader.ReadToEndAsync();
                    var minifiedHtml = MinifyHtml(html);
                    var minifiedBytes = Encoding.UTF8.GetBytes(minifiedHtml);

                    context.Response.ContentLength = minifiedBytes.Length;
                    await context.Response.Body.WriteAsync(minifiedBytes, 0, minifiedBytes.Length);
                }
            }
        }

        private string MinifyHtml(string html)
        {
            return System.Text.RegularExpressions.Regex.Replace(html, @"\s+", " ")
                .Replace("> <", "><") // Xóa khoảng trắng giữa thẻ đóng/mở
                .Trim();
        }
    }
}
