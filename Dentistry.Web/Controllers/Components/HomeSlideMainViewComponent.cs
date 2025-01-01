using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers.Components
{
    public class HomeSlideMainViewComponent : ViewComponent
    {
        public HomeSlideMainViewComponent() { 
        }
        public IViewComponentResult Invoke()
        {
            return View("~/Views/ViewComponents/HomeSlideMain.cshtml");
        }
    }
    
}
