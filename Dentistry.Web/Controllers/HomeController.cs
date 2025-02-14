using Dentistry.Common.Constants;
using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Catalog.Articles;
using Dentistry.ViewModels.Catalog.Home;
using Dentistry.Web.Models;
using Dentisty.Data.Interfaces;
using Dentisty.Data.Repositories;
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
        private readonly IMemoryCache _memoryCache;

        public HomeController(ILogger<HomeController> logger, IAppSettingRepository appSettingRepository,
            IArticleRepository articleRepository, IMemoryCache memoryCache)
        {
            _appSettingRepository = appSettingRepository;
            _logger = logger;
            _articleRepository = articleRepository;
            _memoryCache = memoryCache;
        }

        public async Task<IActionResult> Index()
        {
            var appSetting = await _appSettingRepository.GetById(1);
            return View(appSetting);
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
    }
}
