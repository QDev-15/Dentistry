using Dentistry.ViewModels.Catalog.AppSettings;
using Dentistry.ViewModels.Common;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Admin.Controllers
{
    public class AppSettingController : Controller
    {
        private readonly IAppSettingRepository _appSettingRepository;
        public AppSettingController(IAppSettingRepository appSettingRepository)
        {
            _appSettingRepository = appSettingRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetSetting()
        {
            return ViewComponent("AppSettings");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSetting(AppSettingDataVm model) {
            var result = await _appSettingRepository.Update(model.Setting);
            return Json(new SuccessResult<bool>());
        }
    }
}
