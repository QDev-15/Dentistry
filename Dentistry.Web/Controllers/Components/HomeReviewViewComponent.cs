using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers.Components
{
    public class HomeReviewViewComponent : ViewComponent
    {
        public HomeReviewViewComponent() { 
        }
        public IViewComponentResult Invoke()
        {
            return View("~/Views/ViewComponents/HomeReview.cshtml");
        }
    }
    
}
