using Dentistry.ViewModels.Enums;
using Dentisty.Data.Interfaces;
using Dentisty.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers.Components
{
    public class HomeArticleNewsViewComponent : ViewComponent
    {
        private readonly ApplicationService _app;
        public HomeArticleNewsViewComponent(ApplicationService app) {
            _app = app;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var news = await _app.GetArticlesNews();
            return View("~/Views/ViewComponents/HomeArticleNews.cshtml", news.Take(6).ToList());
        }
    }
    
}
