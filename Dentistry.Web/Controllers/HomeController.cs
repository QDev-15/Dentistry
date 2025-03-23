using Dentistry.Common;
using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Catalog.Articles;
using Dentistry.ViewModels.Catalog.Home;
using Dentistry.Web.Models;
using Dentisty.Data.GeneratorDB.Entities;
using Dentisty.Data.Interfaces;
using Dentisty.Data.Repositories;
using Dentisty.Data.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace Dentistry.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAppSettingRepository _appSettingRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly ICacheService _cacheService;
        private readonly IMemoryCache _memoryCache;
        private readonly ICategoryReposiroty _categoryReposiroty;

        public HomeController(ILogger<HomeController> logger, IAppSettingRepository appSettingRepository,
            IArticleRepository articleRepository, IMemoryCache memoryCache, ICacheService cacheService, ICategoryReposiroty categoryReposiroty)
        {
            _categoryReposiroty = categoryReposiroty;
            _cacheService = cacheService;
            _appSettingRepository = appSettingRepository;
            _logger = logger;
            _articleRepository = articleRepository;
            _memoryCache = memoryCache;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Nhiên Dentistry";
            ViewData["Description"] = $"Trang chủ Nhiên Nha Khoa - Cơ sở uy tín trao gửi niềm tin.";
            ViewData["Keywords"] = "Nhiên, Nha Khoa, Cơ sở uy tín, làm răng, răng sứ";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet("tim-kiem")]
        public async Task<IActionResult> Search(string keyWord, int page = 1) {
            // Lấy dữ liệu từ cache (nếu có)
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
            var result = await _articleRepository.GetForSearch(keyWord, page);
            ViewData["keyWord"] = keyWord;
            ViewData["TotalPages"] = result.PageCount;
            ViewData["CurrentPage"] = page;
            SearchVm model = new SearchVm()
            {
                Items = result.Items,
                HotNews = artsHotNews
            };
            // add seo
            if (result.Items != null && result.Items.Any())
            {
                string titles = string.Join(", ", result.Items.Select(x => x.Title).ToList());
                string tags = string.Join(",", result.Items.Select(x => x.Tags).ToList());
                tags = string.Join(", ", tags.Split(",").Distinct());
                ViewData["Title"] = SystemConstants.ApplicationTitle;
                ViewData["Description"] = $"Đọc ngay bài viết '{titles}' để hiểu hơn về {tags}";
                ViewData["Keywords"] = keyWord ?? SystemConstants.ApplicationTitle;
            }
            return View(model);
        }

        // Phương thức để xóa cache khi nhận tín hiệu từ SignalR
        public void InvalidateCache()
        {
            _memoryCache.Remove("ArtsHotNews");
            Console.WriteLine("Cache đã bị xóa do tín hiệu từ Admin!");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet("refresh-cache/{secret}")]
        public async Task<IActionResult> RemoveCache(string secret)
        {
            if (secret == null)
            {
                return NotFound("Not found secret key");
            }
            else if(secret != "24032024-03031992-81726354") {
                return Ok("Undefield");               
            } else
            {
                _cacheService.RemoveAsync(SystemConstants.CacheKeys.AppSetting);
                _cacheService.RemoveAsync(SystemConstants.CacheKeys.AppCategory);
                _cacheService.RemoveAsync(SystemConstants.CacheKeys.AppBranches);
                _cacheService.RemoveAsync(SystemConstants.CacheKeys.AppSlide);
                _cacheService.RemoveAsync(SystemConstants.CacheKeys.AppSettingDoctor);
                _cacheService.RemoveAsync(SystemConstants.CacheKeys.AppSettingCategory);
                _cacheService.RemoveAsync(SystemConstants.CacheKeys.AppSettingNews);
                _cacheService.RemoveAsync(SystemConstants.CacheKeys.AppSettingProduct);
                _cacheService.RemoveAsync(SystemConstants.CacheKeys.AppSettingFeedback);
                _cacheService.RemoveAsync(SystemConstants.CacheKeys.AppSettingArticle);
                _cacheService.RemoveAsync(SystemConstants.CacheKeys.ArticleChange);
                return Ok("Done");
            }
        }
        [HttpGet("/refresh-category/{secret}")]
        public async Task<IActionResult> RefreshCategory(string secret, int id = 0)
        {
            if (secret == null)
            {
                return NotFound("Not found secret key");
            }
            else if (secret != "24032024-03031992-81726354")
            {
                return Ok("Undefield");
            }
            else
            {
                //if (id > 0)
                //{
                //    for (int i = 0; i < 21; i++)
                //    {
                //        _categoryReposiroty.CreateArticle(id, ViewModels.Enums.ArticleType.FeedBack);
                //        _categoryReposiroty.CreateArticle(id, ViewModels.Enums.ArticleType.Article);
                //        _categoryReposiroty.CreateArticle(id, ViewModels.Enums.ArticleType.Products);
                //        _categoryReposiroty.CreateArticle(id, ViewModels.Enums.ArticleType.News);
                //    }
                //}
                _categoryReposiroty.RefreshCategory();
                return Ok("Done");
            }
        }
    }
}
