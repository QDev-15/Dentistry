﻿using Dentistry.Admin.Common;
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

namespace Dentistry.Admin.Controllers
{
    [Authorize]
    public class ArticlesController : BaseController
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ICategoryReposiroty _categoryReposiroty;
        private readonly IImageRepository _imageRepository;
        private readonly CacheNotificationService _cacheService;


        public ArticlesController(CacheNotificationService cacheNotificationService, IArticleRepository articleRepository, IImageRepository imageRepository, 
            ICategoryReposiroty categoryReposiroty, IHubContext<SignalRHub> hubContext)
        {
            _imageRepository = imageRepository;
            _categoryReposiroty = categoryReposiroty;
            _articleRepository = articleRepository;
            _cacheService = cacheNotificationService;
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
            DateTime date = DateTime.Now;
            model.Item.Alias = model.Item.Title.ToSlus();
            var checkAlis = await _articleRepository.CheckExistsAlias(model.Item);
            if (checkAlis)
            {
                model.Item.Alias = model.Item.Title.ToSlus() + date.GetTimestamp();
                checkAlis = await _articleRepository.CheckExistsAlias(model.Item);
                if (checkAlis)
                {
                    return Json(new { success = false, message = "Tiêu đề đã tồn tại, xin vui lòng chọn lại tiêu đề." });
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
    }
}
