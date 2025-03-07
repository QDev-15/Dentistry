using Dentisty.Data;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers.Components
{
    public class AppToolbarTopViewComponent : ViewComponent
    {
        private readonly IAppSettingRepository _settingRepository;
        public AppToolbarTopViewComponent(IAppSettingRepository appSettingRepository) {
            _settingRepository = appSettingRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var settings = await _settingRepository.GetByIdAsync(1);
            return View("~/Views/ViewComponents/AppToolbarTop.cshtml", settings.ReturnViewModel());
        }
    }
    
}
