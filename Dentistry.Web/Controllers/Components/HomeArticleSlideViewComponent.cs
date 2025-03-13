using Dentisty.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers.Components
{
    public class HomeArticleSlideViewComponent : ViewComponent
    {
        private readonly ApplicationService _app;
        public HomeArticleSlideViewComponent(ApplicationService app) {
            _app = app;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var articles = await _app.GetArticleSlides();
            return View("~/Views/ViewComponents/HomeArticleSlide.cshtml", articles);
        }
    }
    
}
