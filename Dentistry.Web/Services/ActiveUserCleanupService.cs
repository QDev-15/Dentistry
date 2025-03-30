
using Dentistry.Data.GeneratorDB.EF;
using Microsoft.EntityFrameworkCore;

namespace Dentistry.Web.Services
{
    public class ActiveUserCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public ActiveUserCleanupService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<DentistryDbContext>();
                    var threshold = DateTime.UtcNow.AddMinutes(-5);

                    // Xóa trực tiếp trên DB để tăng tốc độ
                    await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM \"ActiveUsers\" WHERE \"LastActive\" < {0}", threshold);
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Chạy mỗi 5 phút
            }
        }
    }
}
