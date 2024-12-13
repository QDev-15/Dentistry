using Dentistry.Admin.Models;
using Dentistry.Common.Constants;
using Dentistry.ViewModels.Catalog;
using Dentistry.ViewModels.Catalog.Slide;
using Dentisty.Data;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Dentistry.Admin.Controllers
{
    [Authorize]
    
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryReposiroty _categoryReposiroty;
        private readonly IImageRepository _imageRepository;
        private readonly ISlideRepository _slideRepository;

        public HomeController(ILogger<HomeController> logger,IImageRepository imageRepository, ISlideRepository slideRepository, ICategoryReposiroty categoryReposiroty)
        {
            _imageRepository = imageRepository;
            _logger = logger;
            _slideRepository = slideRepository;
            _categoryReposiroty = categoryReposiroty;
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
        [HttpGet("cai-dat")]
        public async Task<IActionResult> Settings()
        {
            var settings = new SettingsViewModel();
            var slides = await _slideRepository.GetAllAsync();
            var categories = await _categoryReposiroty.GetAllAsync();
            settings.Slides = slides.Select( x => x.ReturnViewModel()).ToList();
            settings.Categories = categories.Select(x => x.ReturnViewModel()).ToList();
            
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
        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var image = await _imageRepository.CreateAsync(file);
            return Json(image);
        }
    }
}
