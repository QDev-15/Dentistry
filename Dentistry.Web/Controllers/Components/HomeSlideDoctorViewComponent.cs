using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers.Components
{
    public class HomeSlideDoctorViewComponent : ViewComponent
    {
        public HomeSlideDoctorViewComponent() { 
        }
        public IViewComponentResult Invoke()
        {
            return View("~/Views/ViewComponents/HomeSlideDoctor.cshtml");
        }
    }
    
}
