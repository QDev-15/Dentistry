using Dentistry.ViewModels.Catalog.Categories;
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
        public async Task<IActionResult> Detail(string danhmuc, string alias)
        {
            var category = await _categoryReposiroty.GetByAlias(alias);
            if (category == null) { 
                category = new CategoryVm();
                ViewData["Title"] = "Not found";
                return View("Detail", category);
            }

            // SEO ==================
            ViewData["Support"] = category.Type == CategoryType.Support;
            ViewData["Description"] = $"Đọc ngay danh mục '{category.Name}' để hiểu hơn về {category.Alias}";
            ViewData["Keywords"] = category.Alias;
            return View("Detail", category);
        }

        private async Task<CategoryDetailVm> getCategoryDetail(string alias, string? danhmuc)
        {
            if (danhmuc == null)
            {
                danhmuc = alias;
            }
            var categoryDetailVm = new CategoryDetailVm();
            CategoryType categoryType = CategoryType.None;

            switch (danhmuc)
            {
                case var _ when danhmuc == CategoryType.About.GetAliasDisplayName(): // Giới thiệu
                    categoryType = CategoryType.About;
                    break;
                case var _ when danhmuc == CategoryType.advise.GetAliasDisplayName(): // Tư vấn
                    categoryType = CategoryType.advise;
                    break;
                case var _ when danhmuc == CategoryType.FeedBack.GetAliasDisplayName(): // Phản hồi
                    categoryType = CategoryType.FeedBack;
                    break;
                case var _ when danhmuc == CategoryType.News.GetAliasDisplayName(): // Tin tức
                    categoryType = CategoryType.News;
                    break;
                case var _ when danhmuc == CategoryType.Products.GetAliasDisplayName(): // sản phẩm
                    categoryType = CategoryType.Products;
                    break;
                case var _ when danhmuc == CategoryType.Service.GetAliasDisplayName(): // dịch vụ
                    categoryType = CategoryType.Service;
                    break;
                case var _ when danhmuc == CategoryType.Support.GetAliasDisplayName(): // Hỗ trợ
                    categoryType = CategoryType.Support;
                    break;
                default:
                    break;
            }
            if (categoryType == CategoryType.None) return categoryDetailVm;

            
            categoryDetailVm.category = await _categoryReposiroty.GetByAlias(alias);
            categoryDetailVm.articles = (await _articleRepository.GetByCategoryId(categoryDetailVm.category.Id)).ToList();
            categoryDetailVm.hotNews = await _app.GetArticlesHotNews();

            // SEO ==================
            if (categoryDetailVm.category.Id > 0)
            {
                ViewData["Support"] = categoryDetailVm.category.Type == CategoryType.Support;
                ViewData["Title"] = categoryDetailVm.category.Name;
                ViewData["Description"] = $"Đọc ngay danh mục '{categoryDetailVm.category.Name}' để hiểu hơn về {categoryDetailVm.category.Alias}";
                ViewData["Keywords"] = categoryDetailVm.category.Alias;
            }
            return categoryDetailVm;
        }
    }
}
