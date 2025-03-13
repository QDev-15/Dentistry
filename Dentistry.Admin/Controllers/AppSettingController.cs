using Dentistry.Data.Common.Constants;
using Dentistry.ViewModels.Catalog.AppSettings;
using Dentistry.ViewModels.Common;
using Dentisty.Data.Interfaces;
using Dentisty.Data.Services;
using Dentisty.Data.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dentistry.Admin.Controllers
{
    public class AppSettingController : Controller
    {
        private readonly IAppSettingRepository _appSettingRepository;
        private readonly CacheNotificationService _cache;
        public AppSettingController(IAppSettingRepository appSettingRepository, CacheNotificationService cacheService)
        {
            _cache = cacheService;
            _appSettingRepository = appSettingRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateVisitor(int id, bool value)
        {
            await _appSettingRepository.UpdateAssess(id, value);
            return Json(new SuccessResult<bool>());
        }
        [HttpGet]
        public IActionResult GetSetting()
        {
            return ViewComponent("AppSettings");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSetting(AppSettingDataVm model) {
            var result = await _appSettingRepository.Update(model.Setting);
            await _cache.InvalidateCacheAsync(SystemConstants.CacheKeys.AppSetting);
            return Json(new SuccessResult<bool>());
        }
    }
}
