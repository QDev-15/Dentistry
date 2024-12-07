using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Admin.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index(int id)
        {
            return PartialView(id);
        }
    }
}
