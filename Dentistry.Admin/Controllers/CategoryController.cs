using Dentistry.Data.GeneratorDB.Entities;
using Dentistry.ViewModels.Catalog.Categories;
using Dentistry.ViewModels.Catalog.Slide;
using Dentisty.Data;
using Dentisty.Data.Interfaces;
using Dentisty.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryReposiroty _categoryRepository;
        private object mesage;

        public CategoryController(ICategoryReposiroty categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return View(categories);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id, string type) {
            var categoryAddEdit = new CategoryVmAddEdit();
            var parentCategories = new List<CategoryVm>();
            if (type == "sub") // add sub category
            {
                categoryAddEdit.parrents = (await _categoryRepository.GetByParent()).Select(x => x.ReturnViewModel()).ToList();
            }
            if (id == 0)
            {
                categoryAddEdit.item = new CategoryVm() {};
            } else
            {
                categoryAddEdit.item = (await _categoryRepository.GetById(id)).ReturnViewModel();               
            }
            categoryAddEdit.item.IsSub = type == "sub";
            return PartialView("_Partial_Category_AddEdit", categoryAddEdit);
        }
        [HttpPost]
        public async Task<IActionResult> AddEdit(CategoryVmAddEdit model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }

            var checkAlis = await _categoryRepository.CheckExistsAlias(model.item.Alias, model.item.Id);
            if (checkAlis)
            {
                checkAlis = await _categoryRepository.CheckExistsAlias(model.item.Name.ToSlus(), model.item.Id);
                if (checkAlis) {
                    return Json(new { success = false, message = "Tên rút gọn đã tồn tại." });
                }
                model.item.Alias = model.item.Name.ToSlus();
            }
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

            return Json(new { success = true });
        }
    }
}
