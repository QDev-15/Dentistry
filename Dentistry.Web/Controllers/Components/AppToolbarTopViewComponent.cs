using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers.Components
{
    public class AppToolbarTopViewComponent : ViewComponent
    {
        public AppToolbarTopViewComponent() { 
        }
        public IViewComponentResult Invoke()
        {
            return View("~/Views/ViewComponents/AppToolbarTop.cshtml");
        }
    }
    
}
