using Dentistry.ViewModels.Common;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dentistry.Admin.Controllers
{
    public class AccessController : Controller
    {
        private readonly IAccessRepository _accessRepository;
        private readonly IAppSettingRepository _appSetting;
        public AccessController(IAccessRepository accessRepository, IAppSettingRepository appSetting    )
        {
            _accessRepository = accessRepository;
            _appSetting = appSetting;
        }

        public async Task<IActionResult> Index()
        {
            var appsetting = await _appSetting.GetById(1);
            ViewBag.AccessValue = appsetting.TrackVisitors;
            ViewBag.VisitorCount = await _accessRepository.CountVistorLogs();
            ViewBag.ActiveCount = await _accessRepository.CountActiveUsers();
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetVisitorLogs(PagingRequestBase requestBase)
        {
            var result = await _accessRepository.GetVisitorLogs(requestBase);
            return Ok(result);
        }
    }
}
