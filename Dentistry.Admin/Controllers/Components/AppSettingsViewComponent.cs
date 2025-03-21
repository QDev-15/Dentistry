using Dentistry.ViewModels.Catalog.AppSettings;
using Dentistry.ViewModels.Catalog.Contacts;
using Dentistry.ViewModels.Enums;
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
            result.CategoryProducts = await _categoryReposiroty.GetCategoryByType(CategoryType.Products);
            result.Categories = await _categoryReposiroty.GetCategoryByType(CategoryType.Service);
            result.Doctors = _doctorRepository.GetDoctorForAppSettings().Result.ToList();
            result.Articles = await _articleRepository.GetByType(ArticleType.Article);
            result.FeedBacks = await _articleRepository.GetByType(ArticleType.FeedBack);
            result.News = await _articleRepository.GetByType(ArticleType.News);
            result.Products = await _articleRepository.GetByType(ArticleType.Products);
            return View("~/Views/AppSetting/Partial/_get_settings.cshtml", result);
        }
    }
}
