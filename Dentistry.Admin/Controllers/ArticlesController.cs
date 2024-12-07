using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Admin.Controllers
{
    public class ArticlesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
