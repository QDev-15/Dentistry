using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers.Components
{
    public class AppMenuViewComponent : ViewComponent
    {
        public AppMenuViewComponent() { 
        }
        public IViewComponentResult Invoke()
        {
            return View("~/Views/ViewComponents/AppMenu.cshtml");
        }
    }
    
}
