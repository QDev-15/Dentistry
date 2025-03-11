using Dentisty.Data.Interfaces;
using Dentistry.ViewModels.Catalog.AppSettings;
using Microsoft.AspNetCore.Mvc;
using Dentistry.ViewModels.Catalog.Contacts;
using Dentisty.Data.Services.Interfaces;

namespace Dentistry.Web.Controllers.Components
{
    public class OnLoadPageModalViewComponent : ViewComponent
    {
        private readonly IAppSettingRepository _settingRepository;
        private readonly ICacheService _cacheService;
        public OnLoadPageModalViewComponent(ICacheService cacheService, IAppSettingRepository appSettingRepository)
        {
            _settingRepository = appSettingRepository;
            _cacheService = cacheService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            const string onloadKey = "OnLoadPageModal";
            var onloadApp = await _cacheService.GetOrSetAsync(onloadKey, async () =>
            {
                var setting = await _settingRepository.GetById(1);
                return setting;
            });

            return View("~/Views/ViewComponents/_onLoadPageModal.cshtml", onloadApp);
        }
    }
}
