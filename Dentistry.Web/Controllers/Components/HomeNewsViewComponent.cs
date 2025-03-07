using Dentistry.ViewModels.Enums;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers.Components
{
    public class HomeNewsViewComponent : ViewComponent
    {
        private readonly IArticleRepository _articleRepository;
        public HomeNewsViewComponent(IArticleRepository articleRepository) {
            _articleRepository = articleRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var news = await _articleRepository.GetForApplication(ArticleType.News);
            return View("~/Views/ViewComponents/HomeNews.cshtml", news.Take(6).ToList());
        }
    }
    
}
