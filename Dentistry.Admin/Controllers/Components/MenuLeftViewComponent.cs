using Dentistry.Admin.Models;
using Dentisty.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Admin.Controllers.Components
{
    public class MenuLeftViewComponent : ViewComponent
    {

        public MenuLeftViewComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            var menuLeft = new MenuLeftViewModel();
            return View("Default", menuLeft);
        }
    }
}
