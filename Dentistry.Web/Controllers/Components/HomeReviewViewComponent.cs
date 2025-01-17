using Dentistry.ViewModels.Enums;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers.Components
{
    public class HomeReviewViewComponent : ViewComponent
    {
        private readonly IArticleRepository _articleRepository;
        public HomeReviewViewComponent(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var reviews = await _articleRepository.GetForApplication(ArticleType.FeedBack);
            return View("~/Views/ViewComponents/HomeReview.cshtml", reviews.Take(3).ToList());
        }
    }
    
}
