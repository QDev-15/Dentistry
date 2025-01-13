using Dentisty.Data.Interfaces;
using Dentistry.ViewModels.Catalog.AppSettings;
using Microsoft.AspNetCore.Mvc;
using Dentistry.ViewModels.Catalog.Contacts;

namespace Dentistry.Web.Controllers.Components
{
    public class OnLoadPageModalViewComponent : ViewComponent
    {
        public OnLoadPageModalViewComponent()
        {
        }

        public IViewComponentResult Invoke()
        {
            var result = new ContactVm();
            return View("~/Views/ViewComponents/_onLoadPageModal.cshtml", result);
        }
    }
}
