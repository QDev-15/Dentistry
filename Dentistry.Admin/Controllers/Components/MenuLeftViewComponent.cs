using Dentistry.Admin.Models;
using Dentistry.ViewModels.Catalog.Categories;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Admin.Controllers.Components
{
    public class MenuLeftViewComponent : ViewComponent
    {
        private readonly ICategoryReposiroty _categoryReposiroty;
        public MenuLeftViewComponent(ICategoryReposiroty categoryReposiroty)
        {
            _categoryReposiroty = categoryReposiroty;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            var menuLeft = new MenuLeftViewModel();
            // get categories
            var categories = await _categoryReposiroty.GetAllAsync();


            foreach (var item in categories.ToList())
            {
                var newCategory = new CategoryVm()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Alias = item.Alias
                };
                if (item.Parent != null)
                {
                    newCategory.Parent = new CategoryVm()
                    {
                        Id = item.Parent.Id,
                        Name = item.Parent.Name,
                        Alias = item.Parent.Alias,
                        ParentId = item.Parent.Id
                    };
                }
                if (item.Categories != null && item.Categories.Count > 0)
                {
                    newCategory.ChildCategories = new List<CategoryVm>();
                    foreach (var cat in item.Categories)
                    {
                        newCategory.ChildCategories.Add(new CategoryVm()
                        {
                            Id = cat.Id,
                            Alias = cat.Alias,
                            Name = cat.Name,
                            ParentId = cat.ParentId
                        });
                    }
                }
                menuLeft.Categories.Add(newCategory);
            }
            return View("Default", menuLeft);
        }
    }
}
