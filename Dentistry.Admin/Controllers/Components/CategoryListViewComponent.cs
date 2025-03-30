using Dentisty.Data.Interfaces;
using Dentistry.ViewModels.Catalog.Contacts;
using Microsoft.AspNetCore.Mvc;
using Dentistry.ViewModels.Catalog.Categories;
using Dentisty.Data;

namespace Dentistry.Admin.Controllers.Components
{
    public class CategoryListViewComponent : ViewComponent
    {
        private readonly ICategoryReposiroty _categoryRepository;
        public CategoryListViewComponent(ICategoryReposiroty categoryReposiroty)
        {
            _categoryRepository = categoryReposiroty;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = new CategoryVmList();
            result.Parents = _categoryRepository.GetParents().Result.Select(x => x.ReturnViewModel()).ToList();
            result.Childs = _categoryRepository.GetChilds().Result.Select(x => x.ReturnViewModel()).ToList();
            
            return View("~/Views/Category/Partial/_List.cshtml", result);
        }
    }
}
