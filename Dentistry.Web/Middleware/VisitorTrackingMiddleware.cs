using Dentistry.Data.GeneratorDB.EF;
using Dentisty.Data.GeneratorDB.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dentistry.Web.Middleware
{
    public class VisitorTrackingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;

        public VisitorTrackingMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _scopeFactory = scopeFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            // Chỉ xử lý khi request là HEAD và có header vị trí

            var userIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var userAgent = context.Request.Headers["User-Agent"].FirstOrDefault() ?? "unknown";
            // Lấy hoặc tạo VisitorId từ cookie
            string visitorId;
            if (context.Request.Cookies.ContainsKey("VisitorId"))
            {
                visitorId = context.Request.Cookies["VisitorId"];
            }
            else
            {
                visitorId = Guid.NewGuid().ToString();
                context.Response.Cookies.Append("VisitorId", visitorId, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    HttpOnly = false,
                    IsEssential = true
                });
            }
            var now = DateTime.UtcNow;
            var today = now.Date;

            var latHeader = context.Request.Headers["X-Visitor-Latitude"].FirstOrDefault();
            var lngHeader = context.Request.Headers["X-Visitor-Longitude"].FirstOrDefault();

            using (var scope = _scopeFactory.CreateScope())
            {
                try
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<DentistryDbContext>();
                    var appsetting = dbContext.AppSettings.FirstOrDefault();
                    if (appsetting !=null && appsetting.TrackVisitors == false)
                    {
                        await _next(context);
                    }
                    // Kiểm tra xem IP này đã truy cập trong ngày chưa
                    var hasVisitedToday = await dbContext.VisitorLogs.FirstOrDefaultAsync(v => v.VisitorId == visitorId && v.IpAddress == userIp);
                    if (hasVisitedToday == null)
                    {
                        dbContext.VisitorLogs.Add(new VisitorLog { 
                            IpAddress = userIp, 
                            VisitTime = now,
                            VisitorId = visitorId,
                            UserAgent = userAgent,
                            Latitude = latHeader, 
                            Longitude = lngHeader
                        });
                    }
                    else if (lngHeader != null && latHeader != null)
                    {
                        hasVisitedToday.Longitude = lngHeader;
                        hasVisitedToday.Latitude = latHeader;
                        dbContext.VisitorLogs.Update(hasVisitedToday);
                    }
                    await dbContext.SaveChangesAsync();

                    // Kiểm tra và cập nhật danh sách người đang online
                    var existingUser = await dbContext.ActiveUsers.FirstOrDefaultAsync(x => x.VisitorId == visitorId && x.IpAddress == userIp);
                    if (existingUser == null)
                    {
                        dbContext.ActiveUsers.Add(new ActiveUser { 
                            IpAddress = userIp, 
                            LastActive = now,
                            UserAgent = userAgent,
                            Latitude = latHeader,
                            Longitude = lngHeader,
                            IsOnline = true,
                            VisitorId = visitorId
                        });
                    }
                    else if (lngHeader != null && latHeader != null)
                    {
                        existingUser.Longitude = lngHeader;
                        existingUser.Latitude = latHeader;
                        dbContext.VisitorLogs.Update(hasVisitedToday);
                    }
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    // Log lỗi nếu cần (ví dụ: dùng ILogger)
                    Console.WriteLine($"[Error] Middleware failed: {ex.Message}");
                }
            }

            await _next(context);
        }
    }
}
