using Dentistry.ViewModels.Enums;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers.Components
{
    public class HomeSlideServiceViewComponent : ViewComponent
    {
        private readonly IArticleRepository _articleRepository;
        public HomeSlideServiceViewComponent(IArticleRepository articleRepository) {
            _articleRepository = articleRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var articles = await _articleRepository.GetForApplication(ArticleType.Article);
            return View("~/Views/ViewComponents/HomeSlideService.cshtml", articles);
        }
    }
    
}
