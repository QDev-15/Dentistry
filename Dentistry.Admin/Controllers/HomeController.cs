using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Dentistry.Admin.Models;
using Dentistry.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Dentistry.Data.Services;

namespace Dentistry.Admin.Controllers
{
    [Authorize] 
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SlideService _slideService;
        private readonly CategoriesService _categoriesService;

        public HomeController(ILogger<HomeController> logger, SlideService slideService, CategoriesService categoriesService)
        {
            _logger = logger;
            _slideService = slideService;
            _categoriesService = categoriesService;
        }

        public IActionResult Index()
        {
            var user = User.Identity.Name;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        public async Task<IActionResult> Settings()
        {
            var settings = new SettingsViewModel();
            var slides = await _slideService.GetAll();
            settings.Slides = slides.ToList();
            
            return View(settings);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult Language(NavigationViewModel viewModel)
        {
            HttpContext.Session.SetString(Constants.AppSettings.DefaultLanguageId,
                viewModel.CurrentLanguageId);

            return Redirect(viewModel.ReturnUrl);
        }
    }
}
