
using Dentistry.Admin.Models;
using Dentistry.ViewModels.System.Users;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dentistry.Admin.Controllers.Components
{
    public class NavigationViewComponent : ViewComponent
    {
        public NavigationViewComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {         

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
            var navigationVm = new NavigationViewModel()
            {
                CurrentUser = userVm,
            };
            
            return View("Default", navigationVm);
        }
    }
}