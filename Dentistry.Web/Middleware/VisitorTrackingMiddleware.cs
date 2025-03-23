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
            var userIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var now = DateTime.UtcNow;
            var today = now.Date;

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
                    bool hasVisitedToday = await dbContext.VisitorLogs.AnyAsync(v => v.IpAddress == userIp && v.VisitTime >= today);
                    if (!hasVisitedToday)
                    {
                        dbContext.VisitorLogs.Add(new VisitorLog { IpAddress = userIp, VisitTime = now });
                        await dbContext.SaveChangesAsync();
                    }

                    // Kiểm tra và cập nhật danh sách người đang online
                    var existingUser = await dbContext.ActiveUsers.FirstOrDefaultAsync(x => x.IpAddress == userIp);
                    if (existingUser == null)
                    {
                        dbContext.ActiveUsers.Add(new ActiveUser { IpAddress = userIp, LastActive = now });
                        await dbContext.SaveChangesAsync();
                    }
                    else if ((now - existingUser.LastActive).TotalSeconds > 60) // Chỉ cập nhật nếu quá 60s
                    {
                        existingUser.LastActive = now;
                        await dbContext.SaveChangesAsync();
                    }
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
