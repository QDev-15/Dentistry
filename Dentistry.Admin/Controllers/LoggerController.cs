using Dentistry.ViewModels.Catalog.Logger;
using Dentisty.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Admin.Controllers
{
    public class LoggerController : Controller
    {
        private readonly LoggerRepository _loggerRepository; // Thêm Repository hoặc Service để lấy dữ liệu từ DB

        public LoggerController(LoggerRepository loggerRepository)
        {
            _loggerRepository = loggerRepository;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetLogs(LoggerRequrestVm request)
        {
            var result = _loggerRepository.GetLogger(request);

            return Json(new
            {
                recordsTotal = result.Total,
                recordsFiltered = result.Total,
                data = result.Items
            });
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Detail(string id)
        {
            var log = _loggerRepository.GetById(id);
            return PartialView("~/Views/Logger/Detail.cshtml", log);
        }
    }
}
