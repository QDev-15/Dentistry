﻿using Dentistry.ViewModels.Catalog.Contacts;
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
        public async Task<IViewComponentResult> InvokeAsync(bool subportView)
        {
            BookFormVm vm = new BookFormVm();
            vm.SupportView = subportView;
            var branches = await _branchesRepository.GetActive();
            vm.branches = branches.ToList();
            return View("~/Views/ViewComponents/AppContact.cshtml", vm);
        }
    }
    
}
