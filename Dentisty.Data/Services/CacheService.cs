using Dentisty.Data.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        // Phương thức lấy hoặc lưu dữ liệu vào cache
        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> getDataFunc, TimeSpan? absoluteExpirationRelativeToNow = null)
        {
            if (_memoryCache.TryGetValue(key, out T value))
            {
                return value;  // Trả về dữ liệu từ cache nếu có
            }

            value = await getDataFunc();  // Nếu không có trong cache, lấy từ DB
            _memoryCache.Set(key, value, absoluteExpirationRelativeToNow ?? TimeSpan.FromMinutes(30));  // Lưu vào cache
            return value;
        }

        // Lấy dữ liệu từ cache
        public async Task<T> GetAsync<T>(string key)
        {
            _memoryCache.TryGetValue(key, out T value);
            return value;
        }

        // Lưu dữ liệu vào cache
        public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpirationRelativeToNow = null)
        {
            _memoryCache.Set(key, value, absoluteExpirationRelativeToNow ?? TimeSpan.FromMinutes(30));  // Lưu vào cache với thời gian hết hạn
        }
        // 🔹 Hàm làm mới lại cache bằng cách xóa key và đặt lại
        public async Task RefreshAsync<T>(string key, Func<Task<T>> getDataFunc, TimeSpan? absoluteExpirationRelativeToNow = null)
        {
            RemoveAsync(key);
            var newValue = await getDataFunc();
            await SetAsync(key, newValue, absoluteExpirationRelativeToNow);
        }

        public void RemoveAsync(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
