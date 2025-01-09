using Dentistry.Common.Constants;
using Dentistry.ViewModels.Catalog.Articles;
using Dentistry.ViewModels.Catalog.Categories;
using Dentistry.ViewModels.Catalog.Slide;
using Dentistry.ViewModels.Enums;
using Dentisty.Data;
using Dentisty.Data.Interfaces;
using Dentisty.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Admin.Controllers
{
    [Authorize]
    public class ArticlesController : BaseController
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ICategoryReposiroty _categoryReposiroty;
        private readonly IImageRepository _imageRepository;

        public ArticlesController(IArticleRepository articleRepository, IImageRepository imageRepository, ICategoryReposiroty categoryReposiroty)
        {
            _imageRepository = imageRepository;
            _categoryReposiroty = categoryReposiroty;
            _articleRepository = articleRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var articles = await _articleRepository.GetAllAsync();
            return View(articles.Select(x => x.ReturnViewModel()).ToList());
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var articles = await _articleRepository.GetAllAsync();
            return PartialView("~/Views/Articles/Partial/_list.cshtml", articles.Select(x => x.ReturnViewModel()).ToList());
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            var model = new ArticleVmAddEdit();
            if (id == 0)
            {
                var artVm = new ArticleVm()
                {
                    Title = "draft",
                    Alias = "draft-" + Guid.NewGuid(),
                    CategoryId = 1,
                    IsActive = true,
                    IsDraft = true
                };
                var art = await _articleRepository.CreateNew(artVm);
                art.Title = "";
                
                model.Item = art;
            } else
            {
                var artVm = await _articleRepository.GetByIdAsync(id);
                model.Item = artVm.ReturnViewModel();
            }
            model.Categories = (await _categoryReposiroty.GetChilds()).Select(x=> x.ReturnViewModel()).ToList();
            ViewBag.ArticleTypes = EnumExtensions.ToSelectList<ArtisleType>();
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

            if (model.Item.Id == 0)
            {
                // Add slide logic
                var slide = await _articleRepository.CreateNew(model.Item);
            }
            else
            {
                // Update slide logic
                var slide = await _articleRepository.UpdateArticle(model.Item);
            }

            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _articleRepository.DeleteArticle(id);
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
                var art = await _articleRepository.GetByIdAsync(id);
                if (art != null)
                {
                    var image = await _imageRepository.CreateAsync(file, SystemConstants.Folder.Article);
                    art.Images.Add(image);
                    _articleRepository.UpdateAsync(art);
                    await _articleRepository.SaveChangesAsync();
                    return Json(image.ReturnViewModel());
                }
                else
                {
                    return Json(new { path = "" });
                }
            }
            catch (Exception ex) { 
                 return BadRequest(ex.Message);
            }
            

            
        }
    }
}
