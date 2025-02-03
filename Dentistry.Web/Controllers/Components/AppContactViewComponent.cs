using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers.Components
{
    public class AppContactViewComponent : ViewComponent
    {
        private readonly IBranchesRepository _branchesRepository;
        public AppContactViewComponent(IBranchesRepository branchesRepository) {
            _branchesRepository = branchesRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var branches = await _branchesRepository.GetActive();
            return View("~/Views/ViewComponents/AppContact.cshtml", branches);
        }
    }
    
}
