using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers.Components
{
    public class HomeSlideServiceViewComponent : ViewComponent
    {
        public HomeSlideServiceViewComponent() { 
        }
        public IViewComponentResult Invoke()
        {
            return View("~/Views/ViewComponents/HomeSlideService.cshtml");
        }
    }
    
}
