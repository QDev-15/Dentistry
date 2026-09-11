using Dentistry.Common;
using Dentistry.ViewModels.Catalog.AppSettings;
using Dentistry.ViewModels.Common;
using Dentisty.Data.Interfaces;
using Dentisty.Data.Services;
using Dentisty.Data.Services.Email;
using Dentisty.Data.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dentistry.Admin.Controllers
{
    public class AppSettingController : Controller
    {
        private readonly IAppSettingRepository _appSettingRepository;
        private readonly CacheNotificationService _cache;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        public AppSettingController(IAppSettingRepository appSettingRepository, CacheNotificationService cacheService,
            IHttpClientFactory httpClientFactory, IConfiguration configuration, IEmailService emailService)
        {
            _cache = cacheService;
            _appSettingRepository = appSettingRepository;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateVisitor(int id, bool value)
        {
            await _appSettingRepository.UpdateAssess(id, value);
            return Json(new SuccessResult<bool>());
        }
        [HttpPost]
        public async Task<IActionResult> UpdateVisitorLocation(int id, bool value)
        {
            await _appSettingRepository.UpdateLocationTracking(id, value);
            return Json(new SuccessResult<bool>());
        }
        [HttpGet]
        public IActionResult GetSetting()
        {
            return ViewComponent("AppSettings");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSetting(AppSettingDataVm model) {
            var result = await _appSettingRepository.Update(model.Setting);
            await _cache.InvalidateCacheAsync(SystemConstants.Cache_Setting);
            await _cache.InvalidateCacheAsync(SystemConstants.Cache_Category);
            await _cache.InvalidateCacheAsync(SystemConstants.Cache_Doctor);
            await _cache.InvalidateCacheAsync(SystemConstants.Cache_Article);
            await _cache.InvalidateCacheAsync(SystemConstants.Cache_Branches);
            return Json(new SuccessResult<bool>());
        }

        [HttpPost]
        public async Task<IActionResult> TestSmtp([FromBody] SmtpTestSettings model)
        {
            if (model == null)
            {
                return Json(new ErrorResult<bool>("Dữ liệu gửi lên không hợp lệ."));
            }
            // Tests whatever is currently typed on the form (not yet saved) - that's the point
            // of testing before hitting Cập nhật. An empty password falls back to the one
            // already stored in the DB (handled inside SendTestAsync).
            var result = await _emailService.SendTestAsync(model);
            if (result.Success)
            {
                return Json(new SuccessResult<bool>());
            }
            return Json(new ErrorResult<bool>(result.ErrorMessage));
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
