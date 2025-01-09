using Dentistry.ViewModels.Catalog.AppSettings;
using Dentistry.ViewModels.Catalog.Contacts;
using Dentisty.Data;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Admin.Controllers.Components
{
    public class AppSettingsViewComponent : ViewComponent
    {
        private readonly IAppSettingRepository _appSettingRepository;
        private readonly ICategoryReposiroty _categoryReposiroty;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IArticleRepository _articleRepository;
        public AppSettingsViewComponent(IAppSettingRepository appSettingRepository, IArticleRepository articleRepository, ICategoryReposiroty categoryReposiroty, IDoctorRepository doctorRepository)
        {
            _articleRepository = articleRepository;
            _doctorRepository = doctorRepository;
            _appSettingRepository = appSettingRepository;
            _categoryReposiroty = categoryReposiroty;   
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = new AppSettingDataVm();
            var apps = await _appSettingRepository.GetAllAsync();
            if (apps != null && apps.Any()) {               
                result.Setting = apps.Select(x => x.ReturnViewModel()).First();           
            }
            result.Categories = _categoryReposiroty.GetForSettings().Result.ToList();
            result.Doctors = _doctorRepository.GetDoctorForAppSettings().Result.ToList();
            result.Articles = _articleRepository.GetArticleForSetting().Result.ToList();
            result.FeedBacks = _articleRepository.GetFeedBackForSetting().Result.ToList();
            result.News = _articleRepository.GetNewsForSetting().Result.ToList();
            result.Products = _articleRepository.GetProductForSetting().Result.ToList();
            return View("~/Views/AppSetting/Partial/_get_settings.cshtml", result);
        }
    }
}
