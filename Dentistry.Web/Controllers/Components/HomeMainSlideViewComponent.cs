using Dentisty.Data.Interfaces;
using Dentisty.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers.Components
{
    public class HomeMainSlideViewComponent : ViewComponent
    {
        private readonly ApplicationService _app;
        public HomeMainSlideViewComponent(ApplicationService app) {
            _app = app;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var slides = await _app.GetMainSlides();
            return View("~/Views/ViewComponents/HomeMainSlide.cshtml", slides);
        }
    }
    
}
