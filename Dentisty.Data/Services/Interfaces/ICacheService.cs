using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Services.Interfaces
{
    public interface ICacheService
    {
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> getDataFunc, TimeSpan? absoluteExpirationRelativeToNow = null);
        Task<T> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpirationRelativeToNow = null);
        Task RefreshAsync<T>(string key, Func<Task<T>> getDataFunc, TimeSpan? absoluteExpirationRelativeToNow = null);
        void RemoveAsync(string key);
        void RemoveAllAsync();
    }
}
