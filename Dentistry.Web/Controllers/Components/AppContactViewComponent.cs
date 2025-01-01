using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers.Components
{
    public class AppContactViewComponent : ViewComponent
    {
        public AppContactViewComponent() { 
        }
        public IViewComponentResult Invoke()
        {
            return View("~/Views/ViewComponents/AppContact.cshtml");
        }
    }
    
}
