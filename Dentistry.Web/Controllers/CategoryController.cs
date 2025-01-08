using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("dich-vu/{alias}")]
        public IActionResult DichVu(string alias) { 
            return View();
        }
    }
}
