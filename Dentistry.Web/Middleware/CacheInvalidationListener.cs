using Dentisty.Data.Repositories;
using Dentisty.Data.Services.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;

namespace Dentistry.Web.Middleware
{
    public class CacheInvalidationListener
    {
        private readonly ICacheService _cacheService;
        private readonly IServiceProvider _serviceProvider;
        private HubConnection _hubConnection;
        private readonly int _maxRetryAttempts = 10; // Số lần thử kết nối tối đa
        private readonly TimeSpan _retryDelay = TimeSpan.FromSeconds(5); // Khoảng thời gian giữa các lần thử

        public CacheInvalidationListener(ICacheService cacheService, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _cacheService = cacheService;
        }

        public async Task StartListeningAsync(string hubUrl)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On<string>("CacheInvalidated", async (key) =>
            {
                LogCaches("remove caches: " + key, "Remove caches");
                _cacheService.RemoveAsync(key);
            });


            _hubConnection.Closed += async (error) =>
            {
                Console.WriteLine($"Connection closed: {error?.Message}. Trying to reconnect...");
                _ = AttemptReconnectAsync(); // Chạy reconnect mà không chặn ứng dụng
            };

            _ = AttemptReconnectAsync(); // Thử kết nối ban đầu
        }
        private async Task AttemptReconnectAsync()
        {
            while (true)
            {
                try
                {
                    await _hubConnection.StartAsync();
                    Console.WriteLine("Connected to CacheHub!");
                    break; // Thoát vòng lặp nếu kết nối thành công
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Reconnect failed: {ex.Message}. Retrying in {_retryDelay.TotalSeconds} seconds...");
                    await Task.Delay(_retryDelay);
                }
            }
        }
        public void LogCaches(string message, string? title)
        {
            using var scope = _serviceProvider.CreateScope(); // Tạo Scope mới
            var loggerRepository = scope.ServiceProvider.GetRequiredService<LoggerRepository>(); // Lấy LoggerRepository
            loggerRepository.QueueLog(message, title);
        }
    }
}
