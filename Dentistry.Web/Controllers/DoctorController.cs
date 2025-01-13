using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers
{
    public class DoctorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("bac-si/{alias}")]
        public IActionResult Doctor(string alias) {
            return View();
        }
    }
}
