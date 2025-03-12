using Dentistry.Admin.Common;
using Dentisty.Data.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Services
{
    public class CacheNotificationService
    {
        private readonly IHubContext<SignalRHub> _hubContext;
        private readonly ICacheService _cacheService;

        public CacheNotificationService(IHubContext<SignalRHub> hubContext, ICacheService cacheService)
        {
            _hubContext = hubContext;
            _cacheService = cacheService;
        }

        public async Task InvalidateCacheAsync(string key)
        {
            _cacheService.RemoveAsync(key);
            await _hubContext.Clients.All.SendAsync("CacheInvalidated", key);
        }
    }
}
