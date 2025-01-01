using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers.Components
{
    public class HomeCategoryViewComponent : ViewComponent
    {
        public HomeCategoryViewComponent() { 
        }
        public IViewComponentResult Invoke()
        {
            return View("~/Views/ViewComponents/HomeCategory.cshtml");
        }
    }
    
}
