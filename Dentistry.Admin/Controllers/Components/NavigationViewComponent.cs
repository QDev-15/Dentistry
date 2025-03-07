
using Dentistry.Admin.Models;
using Dentistry.Common.Constants;
using Dentisty.Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace Dentistry.Admin.Controllers.Components
{
    public class NavigationViewComponent : ViewComponent
    {
        private readonly LanguagesServices _languagesServices;

        public NavigationViewComponent(LanguagesServices languagesServices)
        {
            _languagesServices = languagesServices;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var languages = await _languagesServices.GetAllLanguagesAsync();
            var currentLanguageId = HttpContext
                .Session
                .GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var items = languages.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = currentLanguageId == null ? x.IsDefault : currentLanguageId == x.Id.ToString()
            });
            var navigationVm = new NavigationViewModel()
            {
                CurrentLanguageId = currentLanguageId,
                Languages = items.ToList()
            };

            return View("Default", navigationVm);
        }
    }
}