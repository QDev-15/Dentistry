
using Dentistry.Admin.Models;
using Dentistry.Common.Constants;
using Dentistry.ViewModels.System.Users;
using Dentisty.Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Security.Claims;
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
                .GetString(Constants.AppSettings.DefaultLanguageId);
            var items = languages.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = currentLanguageId == null ? x.IsDefault : currentLanguageId == x.Id.ToString()
            });
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
            var navigationVm = new NavigationViewModel()
            {
                CurrentLanguageId = currentLanguageId,
                Languages = items.ToList(),
                CurrentUser = userVm,
            };
            
            return View("Default", navigationVm);
        }
    }
}