using Dentistry.ViewModels.Catalog.Categories;
using Dentisty.Data;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryReposiroty _categoryReposiroty;
        private readonly IArticleRepository _articleRepository;
        public CategoryController(ICategoryReposiroty categoryReposiroty, IArticleRepository articleRepository)
        {
            _categoryReposiroty = categoryReposiroty;
            _articleRepository = articleRepository; 
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("danh-muc/{alias}")]
        public async Task<IActionResult> Detail(string alias) {
            var categoryDetailVm = new CategoryDetailVm();
            categoryDetailVm.item = await _categoryReposiroty.GetByAlias(alias);
            categoryDetailVm.articles = (await _articleRepository.GetByCategoryId(categoryDetailVm.item.Id)).ToList();
            return View(categoryDetailVm);
        }
    }
}
