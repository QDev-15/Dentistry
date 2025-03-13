using Dentisty.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers.Components
{
    public class HomeCategorySlideViewComponent : ViewComponent
    {
        private readonly ApplicationService _app;
        public HomeCategorySlideViewComponent(ApplicationService app) {
            _app = app;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _app.GetCategorySlides();
            return View("~/Views/ViewComponents/HomeCategorySlide.cshtml", categories);
        }
    }
    
}
