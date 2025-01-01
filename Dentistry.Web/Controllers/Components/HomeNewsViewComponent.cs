using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers.Components
{
    public class HomeNewsViewComponent : ViewComponent
    {
        public HomeNewsViewComponent() { 
        }
        public IViewComponentResult Invoke()
        {
            return View("~/Views/ViewComponents/HomeNews.cshtml");
        }
    }
    
}
