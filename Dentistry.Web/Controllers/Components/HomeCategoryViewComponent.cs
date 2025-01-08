using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers.Components
{
    public class HomeCategoryViewComponent : ViewComponent
    {
        private readonly ICategoryReposiroty _categoryReposiroty;
        public HomeCategoryViewComponent(ICategoryReposiroty categoryReposiroty) {
            _categoryReposiroty = categoryReposiroty;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _categoryReposiroty.GetFlatHomePage();
            return View("~/Views/ViewComponents/HomeCategory.cshtml", categories.ToList());
        }
    }
    
}
