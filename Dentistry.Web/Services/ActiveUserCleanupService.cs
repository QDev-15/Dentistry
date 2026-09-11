
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
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<DentistryDbContext>();
                        var threshold = DateTime.UtcNow.AddMinutes(-5);

                        // Xóa trực tiếp trên DB để tăng tốc độ
                        await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM \"ActiveUsers\" WHERE \"LastActive\" < {0}", stoppingToken, threshold);
                    }
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    // QUAN TRỌNG: BackgroundService mặc định sẽ làm SẬP TOÀN BỘ ứng dụng nếu
                    // ExecuteAsync ném lỗi không bắt (đã thực sự xảy ra: SQL timeout/deadlock
                    // thoáng qua ở đây từng khiến cả website tắt theo). Task dọn dẹp định kỳ chỉ
                    // nên bỏ qua lượt chạy lỗi và thử lại ở vòng lặp sau, không được phép kéo sập
                    // toàn bộ web vì một lần SQL chậm/timeout.
                    Console.WriteLine($"[Error] ActiveUserCleanupService failed, will retry next cycle: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Chạy mỗi 1 phút
            }
        }
    }
}
