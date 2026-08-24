using Dentistry.Common;
using Dentistry.ViewModels.Common;
using Dentisty.Data.Repositories;
using Dentisty.Data.Storages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Dentistry.Admin.Controllers
{
    [Authorize(Roles = SystemConstants.AdminRoleName)]
    public class BackupController : BaseController
    {
        private readonly DatabaseBackupService _backupService;
        private readonly IWebHostEnvironment _env;
        private readonly HostingConfig _hostingConfig;
        private readonly LoggerRepository _logger;

        public BackupController(DatabaseBackupService backupService, IWebHostEnvironment env,
            IOptions<HostingConfig> hostingConfig, LoggerRepository logger)
        {
            _backupService = backupService;
            _env = env;
            _hostingConfig = hostingConfig.Value;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var backups = _backupService.ListBackups(GetBackupDirectory());
            return View(backups);
        }

        [HttpPost]
        public IActionResult Create()
        {
            try
            {
                var result = _backupService.CreateBackup(GetBackupDirectory());
                return Json(new SuccessResult<DatabaseBackupFileInfo>(result));
            }
            catch (Exception ex)
            {
                var fullMessage = GetFullExceptionMessage(ex);
                _logger.QueueLog(fullMessage, "Backup database");
                return Json(new ErrorResult<DatabaseBackupFileInfo>("Tạo bản sao lưu thất bại: " + fullMessage));
            }
        }

        private static string GetFullExceptionMessage(Exception ex)
        {
            var messages = new List<string>();
            var current = ex;
            while (current != null)
            {
                messages.Add(current.Message);
                current = current.InnerException;
            }
            return string.Join(" --> ", messages);
        }

        [HttpGet]
        public IActionResult Download(string fileName)
        {
            var safeName = Path.GetFileName(fileName);
            var filePath = Path.Combine(GetBackupDirectory(), safeName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }
            return PhysicalFile(filePath, "application/octet-stream", safeName);
        }

        [HttpPost]
        public IActionResult Delete(string fileName)
        {
            var deleted = _backupService.DeleteBackup(GetBackupDirectory(), fileName);
            return Json(deleted ? new SuccessResult<bool>() : new ErrorResult<bool>("Không tìm thấy file."));
        }

        private string GetBackupDirectory()
        {
            var relative = string.IsNullOrWhiteSpace(_hostingConfig.BackupDirectory) ? "db-backups" : _hostingConfig.BackupDirectory;
            return Path.Combine(_env.ContentRootPath, relative);
        }
    }
}
