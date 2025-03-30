using Dentisty.Data.Interfaces;
using Dentistry.ViewModels.Catalog.Contacts;
using Microsoft.AspNetCore.Mvc;
using Dentistry.ViewModels.Catalog.Categories;
using Dentisty.Data;

namespace Dentistry.Admin.Controllers.Components
{
    public class BranchesListViewComponent : ViewComponent
    {
        private readonly IBranchesRepository _branchesRepository;
        public BranchesListViewComponent(IBranchesRepository branchesRepository)
        {
            _branchesRepository = branchesRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _branchesRepository.GetAll();
            return View("~/Views/Branches/Partial/List.cshtml", result);
        }
    }
}
