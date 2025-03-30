using Dentistry.Admin.Models;
using Dentistry.ViewModels.Catalog.Categories;
using Dentistry.ViewModels.System.Users;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            /// get users
            var claimsPrincipal = User as ClaimsPrincipal;
            var userVm = new UserVm
            {
                Id = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value!,
                DisplayName = claimsPrincipal.FindFirst("DisplayName")?.Value!,
                Email = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value!,
                FirstName = claimsPrincipal.FindFirst(ClaimTypes.GivenName)?.Value!,
                UserName = claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value!,
                Roles = claimsPrincipal.FindFirst(ClaimTypes.Role)?.Value!.Split(';'),
            };
            menuLeft.CurrentUser = userVm;
            return View("Default", menuLeft);
        }
    }
}
