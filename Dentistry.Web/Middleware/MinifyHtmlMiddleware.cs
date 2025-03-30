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
                !context.Request.Headers["Accept"].ToString().Contains("text/html", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            var originalBodyStream = context.Response.Body;
            using (var memoryStream = new MemoryStream())
            {
                context.Response.Body = memoryStream;

                await _next(context);

                // Đảm bảo response không bị lỗi hoặc trống
                if (context.Response.StatusCode == 200 && context.Response.ContentType?.Contains("text/html", StringComparison.OrdinalIgnoreCase) == true)
                {
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    using var reader = new StreamReader(memoryStream);
                    var html = await reader.ReadToEndAsync();
                    var minifiedHtml = MinifyHtml(html);
                    var minifiedBytes = Encoding.UTF8.GetBytes(minifiedHtml);

                    // Loại bỏ Content-Length nếu có, để khớp với dụng lượng html cũ và mơ
                    context.Response.Headers.Remove("Content-Length");

                    // Ghi nội dung đã nén vào response body
                    context.Response.Body = originalBodyStream;
                    await context.Response.Body.WriteAsync(minifiedBytes, 0, minifiedBytes.Length);
                }
                else
                {
                    // Nếu không phải text/html hoặc status khác 200, giữ nguyên response gốc
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    await memoryStream.CopyToAsync(originalBodyStream);
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
