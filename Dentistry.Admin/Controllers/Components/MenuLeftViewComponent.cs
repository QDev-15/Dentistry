using Dentistry.Admin.Models;
using Dentistry.Data.Services;
using Dentisty.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Admin.Controllers.Components
{
    public class MenuLeftViewComponent : ViewComponent
    {
        private readonly CategoriesService _categoryService;
        public MenuLeftViewComponent(CategoriesService categoriesService)
        {
            _categoryService = categoriesService;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            var menuLeft = new MenuLeftViewModel();
            // get categories
            var categories = await _categoryService.GetAll();
            menuLeft.Categories = categories.ToList();
            return View("Default", menuLeft);
        }
    }
}
