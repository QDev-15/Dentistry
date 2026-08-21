using Dentistry.ViewModels.Catalog.Categories;
using Dentistry.ViewModels.Common;
using Dentistry.ViewModels.Enums;
using Dentisty.Data.Interfaces;
using Dentisty.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryReposiroty _categoryReposiroty;
        private readonly IArticleRepository _articleRepository;
        private readonly ApplicationService _app;
        public CategoryController(ICategoryReposiroty categoryReposiroty,
            IArticleRepository articleRepository, ApplicationService app)
        {
            _app = app;
            _categoryReposiroty = categoryReposiroty;
            _articleRepository = articleRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("category/{alias}")]
        public async Task<IActionResult> Detail(string alias)
        {
            var category = await _app.GetCategoryByAlias(alias);// _categoryReposiroty.GetByAlias(alias);
            category.Articles = await _app.GetArticlesByCategoryId(category.Id);

            if (category == null) { 
                category = new CategoryVm();
                return View("Detail", category);
            }

            // SEO ==================
            var title = await _app.GetApplicationName() + " - " + category.Name;
            ViewData["Title"] = title;
            ViewData["Support"] = category.Type == CategoryType.Support;
            ViewData["Description"] = $"Đọc ngay danh mục '{category.Name}' để hiểu hơn về {category.Alias}";
            ViewData["Keywords"] = category.Alias;
            ViewData["Image"] = category.CoverImage;
            return View("Detail", category);
        }

        [HttpGet("article/{alias}")]
        public async Task<IActionResult> Article(string alias, int page = 1)
        {
            if (page < 1)
            {
                page = 1;
            }

            var category = await _app.GetCategoryByAlias(alias);
            var result = await _app.GetCategoryArticles(category.Id, page);
            ViewData["TotalPages"] = result.PageCount;
            ViewData["CurrentPage"] = page;
            // SEO ==================
            var title = await _app.GetApplicationName() + " - " + category.Name;
            ViewData["Title"] = title;
            ViewData["Description"] = category.Description;
            ViewData["Keywords"] = category.Alias;
            ViewData["Image"] = category.CoverImage;
            return View("Article", result);
        }
    }
}
