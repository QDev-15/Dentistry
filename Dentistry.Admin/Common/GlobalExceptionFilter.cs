using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Dentisty.Data.Repositories;
using System.Diagnostics;
using System.Reflection;

namespace Dentistry.Admin.Common
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly LoggerRepository _loggerRepository;
        ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger, LoggerRepository loggerRepository)
        {
            _logger = logger;
            _loggerRepository = loggerRepository;
        }

        public void OnException(ExceptionContext context)
        {
            var declaringType = MethodBase.GetCurrentMethod()?.DeclaringType;
            string className = declaringType.FullName ?? "Unknown";
            _loggerRepository.QueueLog(context.Exception.Message, className + ".cs");
            _logger.LogError(context.Exception, "Unhandled exception occurred.");
            context.Result = new ViewResult { ViewName = "Error" }; // Friendly error page
            context.ExceptionHandled = true;
        }
    }
}
