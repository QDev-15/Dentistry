using Dentistry.Web.Models;
using Dentisty.Data.Interfaces;
using Dentisty.Data.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers.Components
{
    public class AppFooterViewComponent : ViewComponent
    {
        private readonly IAppSettingRepository _settingRepository;
        private readonly IBranchesRepository _branchesRepository;
        private readonly ICacheService _cacheService;
        public AppFooterViewComponent(ICacheService cacheService, IAppSettingRepository appSettingRepository, IBranchesRepository branchesRepository) { 
            _branchesRepository = branchesRepository;
            _settingRepository = appSettingRepository;
            _cacheService = cacheService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            const string appFooterKey = "AppFooter";
            var cacheAppFooter = await _cacheService.GetOrSetAsync(appFooterKey, async () =>
            {
                var appFooter = new AppFooter();
                appFooter.setting = await _settingRepository.GetById(1);
                appFooter.branches = (await _branchesRepository.GetActive()).ToList();
                return appFooter;
            });
            
            return View("~/Views/ViewComponents/AppFooter.cshtml", cacheAppFooter);
        }
    }
    
}
