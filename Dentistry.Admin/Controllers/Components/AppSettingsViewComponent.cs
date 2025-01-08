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
        public AppSettingsViewComponent(IAppSettingRepository appSettingRepository, ICategoryReposiroty categoryReposiroty)
        {
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
            result.Categories = (await _categoryReposiroty.GetForSettings()).ToList();
            return View("~/Views/AppSetting/Partial/_get_settings.cshtml", result);
        }
    }
}
