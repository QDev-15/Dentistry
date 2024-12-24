using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
