using Dentisty.Data.Repositories;
using Dentistry.ViewModels.Catalog.Categories;
using Dentistry.ViewModels.Enums;
using Microsoft.AspNetCore.Mvc;
using Dentisty.Data.Interfaces;
using Dentistry.ViewModels.Catalog.Branches;
using Dentistry.ViewModels.Common;

namespace Dentistry.Admin.Controllers
{
    public class BranchesController : BaseController
    {
        private readonly IBranchesRepository _branchesRepository;
        public BranchesController(IBranchesRepository branchesRepository)
        {
            _branchesRepository = branchesRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult List()
        {
            return ViewComponent("BranchesList");
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            var branches = new BranchesVm();
            if (id != 0)
            {
                branches = await _branchesRepository.GetById(id);
            }
            return PartialView("~/Views/Branches/Partial/AddEdit.cshtml", branches);
        }
        [HttpPost]
        public async Task<IActionResult> AddEdit(BranchesVm model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }

            try
            {
                if (model.Id == 0)
                {
                    // Add slide logic
                    var branches = await _branchesRepository.CreateNew(model);
                }
                else
                {
                    // Update slide logic
                    var branches = await _branchesRepository.Update(model);
                }
                return Json(new SuccessResult<bool>());
            }
            catch (Exception ex) {
                return Json(new ErrorResult<bool>(ex.Message));
            }
            

            
        }   
        [HttpPost]
        public async Task<IActionResult> Activated(int id, bool active)
        {
            try
            {
                var result = await _branchesRepository.Active(id, active);
                if (!result)
                {
                    return Json(new ErrorResult<bool>("Cập nhật không thành công."));
                }
                return Json(new SuccessResult<bool>());
            }
            catch (Exception ex) { 
                return Json(new ErrorResult<bool>(ex.Message));            
            }
            
        }
    }
}
