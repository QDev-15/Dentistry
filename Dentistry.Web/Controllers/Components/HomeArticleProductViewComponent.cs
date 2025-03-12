using Dentistry.ViewModels.Enums;
using Dentisty.Data.Interfaces;
using Dentisty.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers.Components
{
    public class HomeArticleProductViewComponent : ViewComponent
    {
        private readonly ApplicationService _app;
        public HomeArticleProductViewComponent(ApplicationService app) {
            _app = app;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var news = await _app.GetArticlesProduct();
            return View("~/Views/ViewComponents/HomeArticleProduct.cshtml", news);
        }
    }
    
}
