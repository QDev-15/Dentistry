using Dentistry.Common;
using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Catalog.Categories;
using Dentistry.ViewModels.Catalog.Slide;
using Dentistry.ViewModels.Common;
using Dentistry.ViewModels.Enums;
using Dentistry.ViewModels.Extensions;
using Dentisty.Data;
using Dentisty.Common;
using Dentisty.Data.Interfaces;
using Dentisty.Data.Repositories;
using Dentisty.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Threading.Tasks;

namespace Dentistry.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryReposiroty _categoryRepository;
        private readonly ICompositeViewEngine _compositeViewEngine;
        private readonly IImageRepository _imageRepository;
        private readonly LoggerRepository _loggerRepository;
        private readonly CacheNotificationService _cacheNotificationService;

        public CategoryController(ICategoryReposiroty categoryRepository, ICompositeViewEngine viewE, IImageRepository imageRepository,
            LoggerRepository loggerRepository, CacheNotificationService cacheNotificationService)
        {
            _imageRepository = imageRepository;
            _categoryRepository = categoryRepository;
            _compositeViewEngine = viewE;
            _loggerRepository = loggerRepository;
            _cacheNotificationService = cacheNotificationService;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return View(categories);
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var categories = await _categoryRepository.GetParents();
            return View("~/Views/Category/Partial/_List_v2.cshtml", categories.Select(x => x.ReturnViewModel()).ToList());
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id, CategoryLevel level, int parentId = 0) {
            var categoryAddEdit = new CategoryVmAddEdit();
            var parentCategories = new List<CategoryVm>();

            categoryAddEdit.item.Level = level;
            categoryAddEdit.item.ParentId = parentId;
            var parent = level == CategoryLevel.Level1;
            if (id == 0)
            {
                categoryAddEdit.item = new CategoryVm() {};
                categoryAddEdit.item.IsParent = level == CategoryLevel.Level1;
                categoryAddEdit.item.Level = level;
                categoryAddEdit.item.ParentId = parentId;
            } else
            {
                categoryAddEdit.item = (await _categoryRepository.GetById(id)).ReturnViewModel();               
            }
            if (parentId > 0)
            {
                categoryAddEdit.item.Parent = (await _categoryRepository.GetById(parentId)).ReturnViewModel();
            }
            categoryAddEdit.CategoryPositions = EnumExtensions.ToSelectList<CategoryPosition>().ToList();
            categoryAddEdit.CategoryType = EnumExtensions.ToSelectList<CategoryType>()
                        .Where(x => (CategoryType)Enum.Parse(typeof(CategoryType), x.Value) != CategoryType.None)
                        .ToList();
            return PartialView("~/Views/Category/Partial/_AddEdit.cshtml", categoryAddEdit);
        }
        [HttpPost]
        public async Task<IActionResult> AddEdit(CategoryVmAddEdit model)
        {
            if (!ModelState.IsValid)
            {
                string html = await this.RenderViewToStringAsync(
                    _compositeViewEngine,
                    "Partial/_AddEdit.cshtml",
                    model
                );
                return Json(new ErrorResult<bool>()
                {
                    data = html,
                    Message = "validate error"
                });
            }

            //model.item.Alias = model.item.Name.ToSlus();
            var checkAlis = await _categoryRepository.CheckExistsAlias(model.item.Alias, model.item.Id);
            if (checkAlis || string.IsNullOrEmpty(model.item.Alias))
            {
                checkAlis = await _categoryRepository.CheckExistsAlias(model.item.Name.ToSlus(), model.item.Id);
                if (checkAlis) {
                    return Json(new ErrorResult<bool>() { Message = "Tên rút gọn đã tồn tại." });
                }
                model.item.Alias = model.item.Name.ToSlus();
            }
            try
            {
                var result = new SuccessResult<bool>();
                if (model.item.Id == 0)
                {
                    // Add category logic
                    var category = await _categoryRepository.CreateNew(model.item);
                    result.data = category;
                }
                else
                {
                    // Update category logic
                    var category = await _categoryRepository.UpdateCategory(model.item);
                    result.data = category;
                }
                await _cacheNotificationService.InvalidateCacheAsync(SystemConstants.Cache_Category);
                
                return Json(result);
            } catch (Exception ex)
            {
                return Json(new ErrorResult<bool>() { Message = ex.Message });
            }
            

            
        }
        [HttpPost]
        public async Task<IActionResult> UploadImage(int id, IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return BadRequest(new { isSuccessed = false, message = "Không có ảnh được tải lên" });
            }
            var result = new SuccessResult<bool>();
            if (id > 0)
            {
                var categoryUpdate = await _categoryRepository.UpLoadFile(id, imageFile);
                result.data = categoryUpdate;
            }


            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var category = await _categoryRepository.GetById(id);
                await _imageRepository.DeleteFile(category.Image);
                _imageRepository.DeleteAsync(category.Image);
                await _categoryRepository.DeleteAsync(category);
                await _cacheNotificationService.InvalidateCacheAsync(SystemConstants.Cache_Category);
                return Json(new SuccessResult<bool>());
            }
            catch (Exception ex)
            {
                return Json(new ErrorResult<bool>() { Message = ex.Message });
            }
        }
    }
}
