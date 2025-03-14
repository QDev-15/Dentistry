﻿using Dentistry.Data.GeneratorDB.EF;
using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Catalog.Logger;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Dentisty.Data.Repositories
{
    public class LoggerRepository
    {
        private readonly DentistryDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<Logger> _loggerRepository;
        private readonly Logs _logQueue;

        public LoggerRepository(IRepository<Logger> repository, IHttpContextAccessor httpContextAccessor, Logs logQueue, DentistryDbContext context)
        {
            _context = context; 
            _httpContextAccessor = httpContextAccessor;
            _loggerRepository = repository;
            _logQueue = logQueue;
        }
        public LoggerResult GetLogger(LoggerRequrestVm request)
        {
            var logs = _context.Loggers.AsQueryable();
            if (!string.IsNullOrEmpty(request.keySearch))
            {
                logs = logs.Where(x => x.Body.Contains(request.keySearch.Trim()));
            }

            // Xác định cột cần sắp xếp
            Expression<Func<Logger, object>> orderByExpression = x => x.CreatedDate; // Mặc định sắp xếp theo CreatedDate

            if (!string.IsNullOrEmpty(request.SortColumn))
            {
                switch (request.SortColumn.ToLower())
                {
                    case "body":
                        orderByExpression = x => x.Body;
                        break;
                    case "title":
                        orderByExpression = x => x.Title;
                        break;
                    case "createddate":
                        orderByExpression = x => x.CreatedDate;
                        break;
                    default:
                        orderByExpression = x => x.CreatedDate;
                        break;
                }
            }

            // Áp dụng sắp xếp theo hướng yêu cầu (asc/desc)
            logs = request.SortDirection == "asc" ? logs.OrderBy(orderByExpression) : logs.OrderByDescending(orderByExpression);



            var result = new LoggerResult();
            result.Total = logs.Count();
            result.Items = logs.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).Select(x => x.ReturnViewModel()).ToList();
            foreach (var item in result.Items)
            {
                item.CreatedDate = item.CreatedDate.ConvertUtcToLocal(_httpContextAccessor);
            }
            return result;
        }
        public LoggerVm GetById(string id)
        {
            var log = _context.Loggers.FirstOrDefault(x => x.Id.ToString() == id).ReturnViewModel();
            log.CreatedDate = log.CreatedDate.ConvertUtcToLocal(_httpContextAccessor);
            return log;
        }
        public string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown";
        }
        public Guid GetCurrentUserGuidId()
        {
            return new Guid(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown");
        }
        public string? GetClientIpAddress()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                return null;

            // Kiểm tra xem có header X-Forwarded-For không
            string? forwardedHeader = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            if (!string.IsNullOrEmpty(forwardedHeader))
            {
                // Nếu có nhiều địa chỉ IP (qua nhiều proxy), lấy IP đầu tiên
                return forwardedHeader.Split(',').FirstOrDefault()?.Trim();
            }

            // Nếu không có proxy, lấy trực tiếp từ RemoteIpAddress
            return httpContext.Connection.RemoteIpAddress?.ToString();
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
