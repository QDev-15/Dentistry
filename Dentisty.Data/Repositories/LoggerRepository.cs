using Dentistry.Data.GeneratorDB.Entities;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.Repositories
{
    public class LoggerRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<Logger> _loggerRepository;
        public LoggerRepository(IRepository<Logger> repository, IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
            _loggerRepository = repository;
        }
        public string? GetClientIpAddress()
        {
            return _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        }
        public void Add(string Message)
        {
            WriteLog(Message, null, null);    
        }
        public void Add(string Message, string title)
        {
            WriteLog(Message, title, null);
        }
        public void Add(string Message, string title, string UserId)
        {
            WriteLog(Message, title, UserId);
        }


        private async void WriteLog(string Message, string? title, string? UserId)
        {
            try
            {
                var logger = new Logger
                {
                    CreatedDate = DateTime.UtcNow,
                    Body = Message,
                    IdAddress = GetClientIpAddress(),
                    Title = title,
                    UserId = UserId
                };
                logger.IdAddress = GetClientIpAddress()!;
                await _loggerRepository.AddAsync(logger);
                await _loggerRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Write log error: ", ex.ToString());
            }
        }
    }
}
