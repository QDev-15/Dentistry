using Dentistry.ViewModels.Catalog.Contacts;
using Dentisty.Data.Interfaces;
using Dentisty.Data.Services.Interfaces;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers.Components
{
    public class AppContactViewComponent : ViewComponent
    {
        private readonly IBranchesRepository _branchesRepository;
        private readonly ICacheService _cacheService;
        public AppContactViewComponent(IBranchesRepository branchesRepository, ICacheService cacheService) {
            _branchesRepository = branchesRepository;
            _cacheService = cacheService;
        }
        public async Task<IViewComponentResult> InvokeAsync(bool subportView)
        {
            const string cacheAppContact = "AppContact";
            var cacheBook = await _cacheService.GetOrSetAsync(cacheAppContact, async () =>
            {
                BookFormVm vm = new BookFormVm();
                vm.SupportView = subportView;
                var branches = await _branchesRepository.GetActive();
                vm.branches = branches.ToList();
                return vm;
            });
            
            return View("~/Views/ViewComponents/AppContact.cshtml", cacheBook);
        }
    }
    
}
