using Dentistry.Common;
using Dentistry.Data.GeneratorDB.EF;
using Dentisty.Data.GeneratorDB.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dentistry.Web.Middleware
{
    public class VisitorTrackingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;

        private static readonly string[] _staticPrefixes =
        [
            "/js/", "/css/", "/lib/", "/plugins/",
            "/assets/", "/uploads/", "/bundle/", "/scss/"
        ];

        public VisitorTrackingMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _scopeFactory = scopeFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value ?? string.Empty;
            if (_staticPrefixes.Any(p => path.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
            {
                await _next(context);
                return;
            }

            var userIp = await Utilities.GetIpAddress();
            var userAgent = context.Request.Headers["User-Agent"].FirstOrDefault() ?? "unknown";

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
            var latHeader = context.Request.Headers["X-Visitor-Latitude"].FirstOrDefault();
            var lngHeader = context.Request.Headers["X-Visitor-Longitude"].FirstOrDefault();

            using (var scope = _scopeFactory.CreateScope())
            {
                try
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<DentistryDbContext>();
                    var appsetting = await dbContext.AppSettings.FirstOrDefaultAsync();
                    if (appsetting != null && appsetting.TrackVisitors == false)
                    {
                        await _next(context);
                        return;
                    }

                    var trackLocation = appsetting == null || appsetting.TrackVisitorLocation;
                    var visitorLat = trackLocation ? latHeader : null;
                    var visitorLng = trackLocation ? lngHeader : null;

                    var hasVisitedToday = await dbContext.VisitorLogs.FirstOrDefaultAsync(v => v.VisitorId == visitorId && v.IpAddress == userIp);
                    if (hasVisitedToday == null)
                    {
                        dbContext.VisitorLogs.Add(new VisitorLog {
                            IpAddress = userIp,
                            VisitTime = now,
                            VisitorId = visitorId,
                            UserAgent = userAgent,
                            Latitude = visitorLat,
                            Longitude = visitorLng
                        });
                    }
                    else if (visitorLat != null && visitorLng != null)
                    {
                        hasVisitedToday.Latitude = visitorLat;
                        hasVisitedToday.Longitude = visitorLng;
                        dbContext.VisitorLogs.Update(hasVisitedToday);
                    }

                    var existingUser = await dbContext.ActiveUsers.FirstOrDefaultAsync(x => x.VisitorId == visitorId && x.IpAddress == userIp);
                    if (existingUser == null)
                    {
                        dbContext.ActiveUsers.Add(new ActiveUser {
                            IpAddress = userIp,
                            LastActive = now,
                            UserAgent = userAgent,
                            Latitude = visitorLat,
                            Longitude = visitorLng,
                            IsOnline = true,
                            VisitorId = visitorId
                        });
                    }
                    else
                    {
                        existingUser.LastActive = now;
                        if (visitorLat != null && visitorLng != null)
                        {
                            existingUser.Latitude = visitorLat;
                            existingUser.Longitude = visitorLng;
                        }
                        dbContext.ActiveUsers.Update(existingUser);
                    }

                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Error] Middleware failed: {ex.Message}");
                }
            }

            await _next(context);
        }
    }
}
