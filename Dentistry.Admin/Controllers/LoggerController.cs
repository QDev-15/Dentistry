using Dentistry.ViewModels.Catalog.Logger;
using Dentisty.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Admin.Controllers
{
    public class LoggerController : BaseController
    {
        private readonly LoggerRepository _loggerRepository; // Thêm Repository hoặc Service để lấy dữ liệu từ DB

        public LoggerController(LoggerRepository loggerRepository)
        {
            _loggerRepository = loggerRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
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
        [HttpGet]
        public IActionResult Detail(string id)
        {
            var log = _loggerRepository.GetById(id);
            return PartialView("~/Views/Logger/Detail.cshtml", log);
        }
    }
}
