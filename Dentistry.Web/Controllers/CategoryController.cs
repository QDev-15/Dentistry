using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("danh-muc/{alias}")]
        public IActionResult Detail(string alias) { 
            return View();
        }
    }
}
