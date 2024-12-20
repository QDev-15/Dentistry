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
        public AppSettingsViewComponent(IAppSettingRepository appSettingRepository) {
            _appSettingRepository = appSettingRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = new AppSettingVm();
            var apps = await _appSettingRepository.GetAllAsync();
            if (apps != null && apps.Any()) {
                result = apps.Select(x => x.ReturnViewModel()).First();           
            }
            return View("~/Views/AppSetting/Partial/_get_settings.cshtml", result);
        }
    }
}
