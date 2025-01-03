using Dentistry.Web.Models;
using Dentisty.Data;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers.Components
{
    public class AppMenuViewComponent : ViewComponent
    {
        private readonly ICategoryReposiroty _categoryReposiroty;
        public AppMenuViewComponent(ICategoryReposiroty categoryReposiroty) { 
            _categoryReposiroty = categoryReposiroty;
        }
        public IViewComponentResult Invoke()
        {
            MenuView menu = new MenuView();
            menu.RightMenu = _categoryReposiroty.GetRightMenuAsync().Result.Select(x => x.ReturnViewModel()).ToList();
            menu.LeftMenu = _categoryReposiroty.GetLeftMenuAsync().Result.Select(x => x.ReturnViewModel()).ToList();
            return View("~/Views/ViewComponents/AppMenu.cshtml", menu);
        }
    }
    
}
