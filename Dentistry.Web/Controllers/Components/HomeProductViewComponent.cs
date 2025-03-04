using Dentistry.ViewModels.Enums;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers.Components
{
    public class HomeProductViewComponent : ViewComponent
    {
        private readonly IArticleRepository _articleRepository;
        public HomeProductViewComponent(IArticleRepository articleRepository) {
            _articleRepository = articleRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var news = await _articleRepository.GetForApplication(ArticleType.Products);
            return View("~/Views/ViewComponents/HomeProduct.cshtml", news.Take(6).ToList());
        }
    }
    
}
