using Dentistry.ViewModels.Catalog.Articles;
using Dentistry.ViewModels.Enums;
using Dentisty.Data;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleRepository _articleRepository;
        public ArticleController(IArticleRepository articleRepository) {
            _articleRepository = articleRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("tim-kiem")]
        public async Task<IActionResult> ListArticle(string keyWord)
        {
            var baiviets = await _articleRepository.GetForSearch(keyWord);
            return View(baiviets);
        }
        [HttpGet("bai-viet/{alias}")]
        public async Task<IActionResult> Articles(string alias)
        {
            var baivietDetail = new ArticleDetailVm();
            var baiviet = await _articleRepository.GetByAliasAsync(alias);
            var baiviets = await _articleRepository.GetForApplication(ArticleType.Article);
            baivietDetail.item = baiviet.ReturnViewModel();
            baivietDetail.items = baiviets.Where(x => x.Id != baiviet.Id).ToList();
            return View(baivietDetail);
        }     
    }
}
