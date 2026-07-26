using Dentistry.Admin.Common;
using Dentistry.Common;
using Dentistry.ViewModels.Catalog.Articles;
using Dentistry.ViewModels.Catalog.Slide;
using Dentistry.ViewModels.Common;
using Dentistry.ViewModels.Enums;
using Dentisty.Data;
using Dentisty.Data.Interfaces;
using Dentisty.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Dentistry.Admin.Controllers
{
    [Authorize]
    public class ArticlesController : BaseController
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ICategoryReposiroty _categoryReposiroty;
        private readonly IImageRepository _imageRepository;
        private readonly CacheNotificationService _cacheService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;


        public ArticlesController(CacheNotificationService cacheNotificationService, IArticleRepository articleRepository, IImageRepository imageRepository,
            ICategoryReposiroty categoryReposiroty, IHubContext<SignalRHub> hubContext, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _imageRepository = imageRepository;
            _categoryReposiroty = categoryReposiroty;
            _articleRepository = articleRepository;
            _cacheService = cacheNotificationService;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var articles = await _articleRepository.GetAllAsync();
            return View(articles.Select(x => x.ReturnViewModel()).ToList());
        }

        [HttpGet]
        public async Task<IActionResult> GetDataTable([FromQuery] DataTableRequest request)
        {
            var result = await _articleRepository.GetListAsync(request);

            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            return PartialView("~/Views/Articles/Partial/_list.cshtml");
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            var model = new ArticleVmAddEdit();
            if (id == 0)
            {
                var artVm = new ArticleVm()
                {
                    CategoryId = 1,
                    IsActive = true,
                    IsDraft = true
                };
                //var art = await _articleRepository.CreateNew(artVm);
                //art.Title = "";
                
                model.Item = artVm;
            } else
            {
                var artVm = await _articleRepository.GetByIdAdminAsync(id);
                model.Item = artVm.ReturnViewModel();
            }
            model.Categories = (await _categoryReposiroty.GetForSettings()).ToList();
            ViewBag.ArticleTypes = EnumExtensions.ToSelectList<ArticleType>();
            return PartialView("~/Views/Articles/Partial/_addEdit.cshtml", model);
        }
        [HttpPost]
        public async Task<IActionResult> AddEdit(ArticleVmAddEdit model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }
            if (model.Item.AvatarFile == null && string.IsNullOrWhiteSpace(model.Item.AvatarUrl))
            {
                var hasAvatar = model.Item.Id != 0 && (await _articleRepository.GetByIdAdminAsync(model.Item.Id))?.Avatar != null;
                if (!hasAvatar)
                {
                    return Json(new ErrorResult<bool>("Vui lòng chọn ảnh đại diện cho bài viết."));
                }
            }
            DateTime date = DateTime.Now;
            model.Item.Alias = model.Item.Title.ToSlus();
            var checkAlis = await _articleRepository.CheckExistsAlias(model.Item);
            if (checkAlis)
            {
                model.Item.Alias = model.Item.Title.ToSlus() + date.GetTimestamp();
                checkAlis = await _articleRepository.CheckExistsAlias(model.Item);
                if (checkAlis)
                {
                    return Json(new ErrorResult<bool>("Tiêu đề đã tồn tại, xin vui lòng chọn lại tiêu đề."));
                }
            }
            var resultArt = new ArticleVm();
            if (model.Item.Id == 0)
            {
                // Add slide logic
                resultArt = await _articleRepository.CreateNew(model.Item);
            }
            else
            {
                // Update slide logic
                resultArt = await _articleRepository.UpdateArticle(model.Item);
            }
            if (model.Item.AvatarFile != null)
            {
                try
                {
                    resultArt = await _articleRepository.UploadAvatar(resultArt.Id, model.Item.AvatarFile);
                }
                catch (Exception ex)
                {
                    return Json(new ErrorResult<bool>("Không thể lưu ảnh đại diện đã tải lên: " + ex.Message));
                }
            }
            else if (!string.IsNullOrWhiteSpace(model.Item.AvatarUrl))
            {
                try
                {
                    resultArt = await _articleRepository.UploadAvatarFromUrl(resultArt.Id, model.Item.AvatarUrl);
                }
                catch (Exception ex)
                {
                    return Json(new ErrorResult<bool>("Không thể tải ảnh đại diện từ đường dẫn đã chọn: " + ex.Message));
                }
            }
            // Gửi tín hiệu tới website để xóa cache
            await _cacheService.InvalidateCacheAsync(SystemConstants.Cache_Article);
            await _cacheService.InvalidateCacheAsync(SystemConstants.Cache_Category);
            return Json(new SuccessResult<bool>());
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var article = await _articleRepository.GetByIdAdminAsync(id);
            var result = await _articleRepository.DeleteArticle(article);

            await _cacheService.InvalidateCacheAsync(SystemConstants.Cache_Article);
            return Json(new { success = result });
        }

        [HttpPost("article-upload-image")]
        public async Task<IActionResult> UploadImage(IFormFile file, int id)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded.");
                }
                var image = await _imageRepository.CreateAsync(file, SystemConstants.Folder.Article);
                await _imageRepository.SaveChangesAsync();
                var imgVm = image.ReturnViewModel();
                imgVm.UploadType = "article";
                return Json(imgVm);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



        }

        [HttpGet]
        public async Task<IActionResult> SearchUnsplash(string query, int page = 1)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Json(Array.Empty<object>());
            }
            try
            {
                var accessKey = _configuration["Unsplash:AccessKey"];
                var client = _httpClientFactory.CreateClient();
                var url = $"https://api.unsplash.com/search/photos?query={Uri.EscapeDataString(query)}&page={page}&per_page=12&client_id={accessKey}";
                var response = await client.GetFromJsonAsync<UnsplashSearchResult>(url);
                var results = (response?.Results ?? new List<UnsplashPhoto>()).Select(x => new
                {
                    id = x.Id,
                    thumb = x.Urls?.Thumb,
                    regular = x.Urls?.Regular,
                    description = x.AltDescription ?? x.Description ?? ""
                });
                return Json(results);
            }
            catch (Exception ex)
            {
                return BadRequest("Không thể tìm kiếm ảnh trên Unsplash: " + ex.Message);
            }
        }

        private class UnsplashSearchResult
        {
            [JsonPropertyName("results")]
            public List<UnsplashPhoto> Results { get; set; } = new();
        }

        private class UnsplashPhoto
        {
            [JsonPropertyName("id")]
            public string Id { get; set; } = "";
            [JsonPropertyName("alt_description")]
            public string? AltDescription { get; set; }
            [JsonPropertyName("description")]
            public string? Description { get; set; }
            [JsonPropertyName("urls")]
            public UnsplashUrls? Urls { get; set; }
        }

        private class UnsplashUrls
        {
            [JsonPropertyName("thumb")]
            public string? Thumb { get; set; }
            [JsonPropertyName("regular")]
            public string? Regular { get; set; }
        }
    }
}
