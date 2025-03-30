using Microsoft.AspNetCore.SignalR;

namespace Dentistry.Admin.Common
{
    public class SignalRHub : Hub
    {
        public async Task NotifyCacheInvalidation(string key)
        {
            await Clients.All.SendAsync("CacheInvalidated", key);
        }
    }
}
