using Dentistry.Web.Models;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Dentistry.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAppSettingRepository _appSettingRepository;

        public HomeController(ILogger<HomeController> logger, IAppSettingRepository appSettingRepository)
        {
            _appSettingRepository = appSettingRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var appSetting = await _appSettingRepository.GetById(1);
            return View(appSetting);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search(string keyWord) {
            return Json(new { success = true });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
