using Dentistry.ViewModels.Catalog.Articles;
using Dentistry.ViewModels.Catalog.Categories;
using Dentistry.ViewModels.Enums;
using Dentisty.Data;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace Dentistry.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryReposiroty _categoryReposiroty;
        private readonly IArticleRepository _articleRepository;
        private readonly IMemoryCache _memoryCache;
        public CategoryController(ICategoryReposiroty categoryReposiroty, IArticleRepository articleRepository, IMemoryCache memoryCache)
        {
            _categoryReposiroty = categoryReposiroty;
            _articleRepository = articleRepository;
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("/{alias}")]
        public async Task<IActionResult> DetailAlias(string alias)
        {
            var categoryDetailVm = await getCategoryDetail(alias, null);

            // SEO ==================
            if (categoryDetailVm.category.Id > 0)
            {
                ViewData["Title"] = categoryDetailVm.category.Name;
                ViewData["Description"] = $"Đọc ngay danh mục '{categoryDetailVm.category.Name}' để hiểu hơn về {categoryDetailVm.category.Alias}";
                ViewData["Keywords"] = categoryDetailVm.category.Alias;
            }
            return View("Detail", categoryDetailVm);
        }
        [HttpGet("{danhmuc}/{alias}")]
        public async Task<IActionResult> Detail(string danhmuc, string alias)
        {
            if (danhmuc == alias)
            {
                return Redirect($"/{alias}");
            }
            var categoryDetailVm = await getCategoryDetail(alias, danhmuc);

            // SEO ==================
            if (categoryDetailVm.category.Id > 0)
            {
                ViewData["Title"] = categoryDetailVm.category.Name;
                ViewData["Description"] = $"Đọc ngay danh mục '{categoryDetailVm.category.Name}' để hiểu hơn về {categoryDetailVm.category.Alias}";
                ViewData["Keywords"] = categoryDetailVm.category.Alias;
            }
            return View("Detail", categoryDetailVm);
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

            if (!_memoryCache.TryGetValue("ArtsHotNews", out List<ArticleVm> artsHotNews))
            {
                artsHotNews = await _articleRepository.GetArticleNew();

                // Lưu vào cache trong 10 phút
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                    SlidingExpiration = TimeSpan.FromMinutes(5),
                    PostEvictionCallbacks =
                    {
                        new PostEvictionCallbackRegistration
                        {
                            EvictionCallback = (key, value, reason, state) =>
                            {
                                Console.WriteLine($"Cache '{key}' bị xóa do: {reason}");
                            }
                        }
                    }
                };
                _memoryCache.Set("ArtsHotNews", artsHotNews, cacheOptions);
            }
            categoryDetailVm.category = await _categoryReposiroty.GetByAlias(alias);
            categoryDetailVm.articles = (await _articleRepository.GetByCategoryId(categoryDetailVm.category.Id)).ToList();
            categoryDetailVm.hotNews = artsHotNews;
            return categoryDetailVm;
        }
    }
}
