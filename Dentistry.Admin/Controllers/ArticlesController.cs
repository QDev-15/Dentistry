using Dentisty.Data;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Admin.Controllers
{
    [Authorize]
    public class ArticlesController : BaseController
    {
        private readonly IArticleRepository _articleRepository;

        public ArticlesController(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var articles = await _articleRepository.GetAllAsync();
            return View(articles.Select(x => x.ReturnViewModel()).ToList());
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var articles = await _articleRepository.GetAllAsync();
            return PartialView("_Partial_Article_list", articles.Select(x => x.ReturnViewModel()).ToList());
        }
        [HttpGet]
        public async Task<IActionResult> ArticleDetail(string alias)
        {
            var article = await _articleRepository.GetByAliasAsync(alias);
            return View(article);
        }
    }
}
