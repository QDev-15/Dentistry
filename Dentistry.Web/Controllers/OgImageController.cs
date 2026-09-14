using Dentisty.Data.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Dentistry.Web.Controllers
{
    // Cắt ảnh cover (kích thước/tỉ lệ tuỳ ý) về đúng kích thước khai trong og:image:width/height,
    // giống cách vnexpress.net dùng CDN riêng (?w=1200&h=675&fit=crop). Ảnh gốc upload chỉ giới
    // hạn cạnh dài <=1200px và giữ nguyên tỉ lệ (xem FTPUpload.cs), nên gần như không bao giờ khớp
    // sẵn 1200x630 - một số trình lấy preview link (nghi có Zalo bản PC) hiện tạm preview rồi tự
    // huỷ khi phát hiện ảnh thật không khớp kích thước đã khai.
    public class OgImageController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICacheService _cache;

        public OgImageController(IHttpClientFactory httpClientFactory, ICacheService cache)
        {
            _httpClientFactory = httpClientFactory;
            _cache = cache;
        }

        [HttpGet("og-image")]
        public async Task<IActionResult> Get(string src, int w = 1200, int h = 630)
        {
            if (string.IsNullOrWhiteSpace(src) ||
                !Uri.TryCreate(src, UriKind.Absolute, out var uri) ||
                (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            {
                return NotFound();
            }

            // Chỉ cho phép crop ảnh của chính site này (thuộc /uploads/...) - tránh biến endpoint
            // thành proxy tải ảnh từ URL bất kỳ do người ngoài truyền vào (rủi ro SSRF).
            if (!string.Equals(uri.Host, Request.Host.Host, StringComparison.OrdinalIgnoreCase) ||
                !uri.AbsolutePath.StartsWith("/uploads/", StringComparison.OrdinalIgnoreCase))
            {
                return NotFound();
            }

            w = Math.Clamp(w, 100, 2000);
            h = Math.Clamp(h, 100, 2000);

            var cacheKey = $"og-image:{uri}:{w}x{h}";
            byte[]? bytes;
            try
            {
                bytes = await _cache.GetOrSetAsync(cacheKey, async () =>
                {
                    using var client = _httpClientFactory.CreateClient();
                    client.Timeout = TimeSpan.FromSeconds(10);
                    var sourceBytes = await client.GetByteArrayAsync(uri);

                    using var image = Image.Load(sourceBytes);
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Crop,
                        Position = AnchorPositionMode.Center,
                        Size = new Size(w, h)
                    }));

                    using var ms = new MemoryStream();
                    image.Save(ms, new JpegEncoder { Quality = 85 });
                    return ms.ToArray();
                }, TimeSpan.FromDays(7));
            }
            catch
            {
                // Nguồn lỗi/không tải được ảnh - để crawler tự lấy ảnh gốc thay vì báo lỗi 500.
                return Redirect(uri.ToString());
            }

            Response.Headers.CacheControl = "public, max-age=604800";
            return File(bytes!, "image/jpeg");
        }
    }
}
