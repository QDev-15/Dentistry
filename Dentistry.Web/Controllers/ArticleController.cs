using Dentistry.Common.Constants;
using Dentistry.Data.GeneratorDB.Entities;
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
        [HttpGet("bai-viet/{alias}")]
        public async Task<IActionResult> Articles(string alias)
        {
            var baivietDetail = new ArticleDetailVm();
            var baiviet = await _articleRepository.GetByAliasAsync(alias);
            var baiviets = await _articleRepository.GetForApplication(ArticleType.Article);
            baivietDetail.item = baiviet.ReturnViewModel();
            baivietDetail.items = baiviets.Where(x => x.Id != baiviet.Id).ToList();

            ViewData["Title"] = baiviet.Title;
            ViewData["Description"] = $"Đọc ngay bài viết '{baiviet.Title}' để hiểu hơn về {baiviet.Tags}";
            ViewData["Keywords"] = baiviet.Tags;
            return View(baivietDetail);
        }     
    }
}
