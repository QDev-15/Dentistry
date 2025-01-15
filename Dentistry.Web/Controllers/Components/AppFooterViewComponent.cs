using Dentistry.Web.Models;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers.Components
{
    public class AppFooterViewComponent : ViewComponent
    {
        private readonly IAppSettingRepository _settingRepository;
        private readonly IBranchesRepository _branchesRepository;
        public AppFooterViewComponent(IAppSettingRepository appSettingRepository, IBranchesRepository branchesRepository) { 
            _branchesRepository = branchesRepository;
            _settingRepository = appSettingRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var appFooter = new AppFooter();
            appFooter.setting = await _settingRepository.GetById(1);
            appFooter.branches = (await _branchesRepository.GetActive()).ToList();
            return View("~/Views/ViewComponents/AppFooter.cshtml", appFooter);
        }
    }
    
}
