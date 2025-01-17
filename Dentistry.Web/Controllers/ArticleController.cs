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
            return View(baivietDetail);
        }     
        [HttpGet("tin-tuc/{alias}")]
        public async Task<IActionResult> NewsAsync(string alias)
        {
            var tintucDetail = new ArticleDetailVm();
            var tintuc = await _articleRepository.GetByAliasAsync(alias);
            var tintucs = await _articleRepository.GetForApplication(ArticleType.News);
            tintucDetail.item = tintuc.ReturnViewModel();
            tintucDetail.items = tintucs.Where(x => x.Id != tintuc.Id).ToList();
            return View(tintucDetail);
        }    
        [HttpGet("phan-hoi/{alias}")]
        public async Task<IActionResult> FeedBack(string alias)
        {
            var phanhoiDetail = new ArticleDetailVm();
            var phanhoi = await _articleRepository.GetByAliasAsync(alias);
            var phanhois = await _articleRepository.GetForApplication(ArticleType.FeedBack);
            phanhoiDetail.item = phanhoi.ReturnViewModel();
            phanhoiDetail.items = phanhois.Where(x => x.Id != phanhoi.Id).ToList();
            return View(phanhoiDetail);
        }    
        [HttpGet("san-pham/{alias}")]
        public async Task<IActionResult> Product(string alias)
        {
            var sanphamDetail = new ArticleDetailVm();
            var sanpham = await _articleRepository.GetByAliasAsync(alias);
            var sanphams = await _articleRepository.GetForApplication(ArticleType.Products);
            sanphamDetail.item = sanpham.ReturnViewModel();
            sanphamDetail.items = sanphams.Where(x => x.Id != sanpham.Id).ToList();
            return View(sanphamDetail);
        }
    }
}
