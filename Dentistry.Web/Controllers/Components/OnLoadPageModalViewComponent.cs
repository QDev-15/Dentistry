using Dentisty.Data.Interfaces;
using Dentistry.ViewModels.Catalog.AppSettings;
using Microsoft.AspNetCore.Mvc;
using Dentistry.ViewModels.Catalog.Contacts;

namespace Dentistry.Web.Controllers.Components
{
    public class OnLoadPageModalViewComponent : ViewComponent
    {
        private readonly IAppSettingRepository _settingRepository;
        public OnLoadPageModalViewComponent(IAppSettingRepository appSettingRepository)
        {
            _settingRepository = appSettingRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var setting = await _settingRepository.GetById(1);
            if (setting != null) {
                ViewData["Hotline"] = setting?.HotlineHaNoi;
            }

            return View("~/Views/ViewComponents/_onLoadPageModal.cshtml");
        }
    }
}
