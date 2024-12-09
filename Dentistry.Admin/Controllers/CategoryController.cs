using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Admin.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index(int id)
        {
            return View(id);
        }
        public IActionResult List(int id) {

            return PartialView("List", id);
        }
    }
}
