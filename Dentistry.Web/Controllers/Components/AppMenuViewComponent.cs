using Dentistry.Web.Models;
using Dentisty.Data;
using Dentisty.Data.Interfaces;
using Dentisty.Data.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers.Components
{
    public class AppMenuViewComponent : ViewComponent
    {
        private readonly ICategoryReposiroty _categoryReposiroty;
        private readonly ICacheService _cacheService;
        public AppMenuViewComponent(ICategoryReposiroty categoryReposiroty, ICacheService cacheService) { 
            _categoryReposiroty = categoryReposiroty;
            _cacheService = cacheService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            const string cacheKey = "AppMenu";
            var settings = await _cacheService.GetOrSetAsync(cacheKey, async () =>
            {
                MenuView menu = new MenuView();
                var rightMenu = await _categoryReposiroty.GetRightMenuAsync();
                var leftMenu = await _categoryReposiroty.GetLeftMenuAsync();
                menu.RightMenu = rightMenu.Select(x => x.ReturnViewModel()).OrderBy(x => x.Sort).ToList();
                menu.LeftMenu = leftMenu.Select(x => x.ReturnViewModel()).OrderBy(x => x.Sort).ToList();
                return menu;
            });
            
            return View("~/Views/ViewComponents/AppMenu.cshtml", settings);
        }
    }
    
}
