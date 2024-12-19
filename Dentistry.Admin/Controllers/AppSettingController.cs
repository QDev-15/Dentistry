using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Admin.Controllers
{
    public class AppSettingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetSetting()
        {
            return PartialView("~/Views/AppSetting/Partial/_get_settings.cshtml", new { });
        }
    }
}
