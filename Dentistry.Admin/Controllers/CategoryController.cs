using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Catalog.Categories;
using Dentistry.ViewModels.Catalog.Slide;
using Dentistry.ViewModels.Common;
using Dentistry.ViewModels.Enums;
using Dentistry.ViewModels.Extensions;
using Dentisty.Data;
using Dentisty.Data.Common;
using Dentisty.Data.Interfaces;
using Dentisty.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Dentistry.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryReposiroty _categoryRepository;
        private readonly ICompositeViewEngine _compositeViewEngine;
        private readonly LoggerRepository _loggerRepository;

        public CategoryController(ICategoryReposiroty categoryRepository, ICompositeViewEngine viewE, LoggerRepository loggerRepository)
        {
            _categoryRepository = categoryRepository;
            _compositeViewEngine = viewE;
            _loggerRepository = loggerRepository;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return View(categories);
        }
        [HttpGet]
        public IActionResult List()
        {
            return ViewComponent("CategoryList");
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id, bool parent) {
            var categoryAddEdit = new CategoryVmAddEdit();
            var parentCategories = new List<CategoryVm>();

            if (id == 0)
            {
                categoryAddEdit.item = new CategoryVm() {};
                categoryAddEdit.item.IsParent = parent;
            } else
            {
                categoryAddEdit.item = (await _categoryRepository.GetById(id)).ReturnViewModel();               
            }
            if (!categoryAddEdit.item.IsParent) // add sub category
            {
                categoryAddEdit.parrents = (await _categoryRepository.GetParents()).Select(x => x.ReturnViewModel()).ToList();
            }
            var categoryTypes = await _categoryRepository.GetCategoryParentTypes();
            var listCategoryTypes = EnumExtensions.ToSelectList<CategoryType>();  // Type mặc định
            var existsCategorys = categoryTypes.Select(x => (int)x); // Type đã tồn tại
            if (categoryAddEdit.item != null)
            {
                existsCategorys = existsCategorys.Where(x => x != (int)categoryAddEdit.item.Type).ToList();
            }
            if (parent)
            {
                listCategoryTypes = listCategoryTypes.Where(x => !existsCategorys.Contains(int.Parse(x.Value))).ToList();

            }
            categoryAddEdit.CategoryPositions = EnumExtensions.ToSelectList<CategoryPosition>();
            categoryAddEdit.CategoryType = listCategoryTypes;
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
                if (model.item.Id == 0)
                {
                    // Add slide logic
                    var slide = await _categoryRepository.CreateNew(model.item);
                }
                else
                {
                    // Update slide logic
                    var slide = await _categoryRepository.UpdateCategory(model.item);
                }
                return Json(new SuccessResult<bool>());
            } catch (Exception ex)
            {
                return Json(new ErrorResult<bool>() { Message = ex.Message });
            }
            

            
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _categoryRepository.DeleteAsync(id);
                return Json(new SuccessResult<bool>());
            }
            catch (Exception ex)
            {
                return Json(new ErrorResult<bool>() { Message = ex.Message });
            }
        }
    }
}
