using Dentistry.Data.GeneratorDB.Entities;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Repositories
{
    public class LoggerRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<Logger> _loggerRepository;
        private readonly Logs _logQueue;

        public LoggerRepository(IRepository<Logger> repository, IHttpContextAccessor httpContextAccessor, Logs logQueue)
        {
            _httpContextAccessor = httpContextAccessor;
            _loggerRepository = repository;
            _logQueue = logQueue;
        }
        public string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown";
        }
        public string? GetClientIpAddress()
        {
            return _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        }

        public async Task Add(string Message, string? Title)
        {
            var userId = GetCurrentUserId() ?? "";
            var logger = new Logger
            {
                CreatedDate = DateTime.UtcNow,
                Body = Message,
                Title = Title ?? "",
                UserId = userId,
                IdAddress = GetClientIpAddress() ?? "Unknown"
            };

            await _loggerRepository.AddAsync(logger);
            await _loggerRepository.SaveChangesAsync();
        }
        public void QueueLog(string message, string? title = null, string? userId = null)
        {
            var logger = new Logger
            {
                CreatedDate = DateTime.UtcNow,
                Body = message,
                Title = title ?? "",
                UserId = userId ?? GetCurrentUserId() ?? "Unknown",
                IdAddress = GetClientIpAddress() ?? "Unknown"
            };

            _logQueue.Enqueue(logger);
        }
    }
}
