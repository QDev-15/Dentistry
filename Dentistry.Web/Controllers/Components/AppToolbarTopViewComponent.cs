using Dentisty.Data;
using Dentisty.Data.Interfaces;
using Dentisty.Data.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers.Components
{
    public class AppToolbarTopViewComponent : ViewComponent
    {
        private readonly IAppSettingRepository _settingRepository;
        private readonly ICacheService _cacheService;
        public AppToolbarTopViewComponent(IAppSettingRepository appSettingRepository, ICacheService cacheService) {
            _settingRepository = appSettingRepository;
            _cacheService = cacheService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            const string cacheKey = "AppToolbarTop";
            var settings = await _cacheService.GetOrSetAsync(cacheKey, async () =>
            {
                return await _settingRepository.GetByIdAsync(1);
            });
            return View("~/Views/ViewComponents/AppToolbarTop.cshtml", settings.ReturnViewModel());
        }
    }
    
}
