using Dentistry.Common.Constants;
using Dentistry.Web.Models;
using Dentisty.Data.Interfaces;
using Dentisty.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Dentistry.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAppSettingRepository _appSettingRepository;
        private readonly IArticleRepository _articleRepository;

        public HomeController(ILogger<HomeController> logger, IAppSettingRepository appSettingRepository, IArticleRepository articleRepository)
        {
            _appSettingRepository = appSettingRepository;
            _logger = logger;
            _articleRepository = articleRepository; 
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
        [HttpPost("tim-kiem")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(string keyWord) {
            var arts = await _articleRepository.GetForSearch(keyWord);
            if (arts != null && arts.Any())
            {
                ViewData["Title"] = SystemConstants.ApplicationTitle;
                ViewData["Description"] = $"Đọc ngay bài viết '{arts[0].Title}' để hiểu hơn về {arts[0].Tags}";
                ViewData["Keywords"] = keyWord ?? SystemConstants.ApplicationTitle;
            }
            return View(arts);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
