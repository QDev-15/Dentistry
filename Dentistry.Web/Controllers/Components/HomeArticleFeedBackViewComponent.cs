using Dentistry.ViewModels.Enums;
using Dentisty.Data.Interfaces;
using Dentisty.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers.Components
{
    public class HomeArticleFeedBackViewComponent : ViewComponent
    {
        private readonly ApplicationService _app;
        public HomeArticleFeedBackViewComponent(ApplicationService app)
        {
            _app = app;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var reviews = await _app.GetArticlesFeddback();
            return View("~/Views/ViewComponents/HomeArticleFeedBack.cshtml", reviews);
        }
    }
    
}
