using Dentisty.Data;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Admin.Controllers.Components
{
    public class ArticleListViewComponent : ViewComponent
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleListViewComponent(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }   
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var _doctors = await _articleRepository.GetAllAsync();
            return View("~/Views/Articles/Partial/_list.cshtml", _doctors.Select(x => x.ReturnViewModel()).ToList());
        }
    }
}
